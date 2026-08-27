namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

public sealed partial class ResultValidatorTests
{
    [TestMethod]
    public void ClearErrors_RemovesPreviouslyMappedErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[] { Error.Validation("Name", "Required.") };
        var result = Result<string>.Failure(errors);
        cut.Instance.HandleResult(result);

        // Act
        cut.Instance.ClearErrors();

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(messages);
    }

    [TestMethod]
    public void HandleResult_CalledTwice_ClearsPreviousErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var firstResult = Result<string>.Failure([Error.Validation("Name", "First error.")]);
        var secondResult = Result<string>.Success("ok");

        cut.Instance.HandleResult(firstResult);

        // Act
        cut.Instance.HandleResult(secondResult);

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(messages);
    }

    [TestMethod]
    public void OnFieldChanged_ClearsErrorsForChangedField()
    {
        // Arrange
        var model = new TestModel();
        var editContext = new EditContext(model);

        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        var errors = new Error[] { Error.Validation("Name", "Required.") };
        var result = Result<string>.Failure(errors);
        cut.Instance.HandleResult(result);

        // Act - simulate field change
        editContext.NotifyFieldChanged(editContext.Field("Name"));

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(messages);
    }

    [TestMethod]
    public void Dispose_UnsubscribesFromFieldChanged()
    {
        // Arrange
        var model = new TestModel();
        var editContext = new EditContext(model);

        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        var errors = new Error[] { Error.Validation("Name", "Error.") };
        cut.Instance.HandleResult(Result<string>.Failure(errors));

        // Act
        cut.Instance.Dispose();

        // Simulate field change after dispose - errors should remain since handler is unsubscribed
        editContext.NotifyFieldChanged(editContext.Field("Name"));

        // Assert - the error persists because the handler was removed
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Error.", messages);
    }

    [TestMethod]
    public void HandleResult_WithFieldSelector_MapsToCustomField()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[] { Error.Validation("account_name", "Name is required.") };
        var result = Result<string>.Failure(errors);

        // Act
        cut.Instance.HandleResult(result, fieldSelector: e => "Name");

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Name is required.", messages);
    }

    [TestMethod]
    public void HandleResult_WithFieldSelector_OverridesDefaultBehaviorForNonValidationErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[] { Error.Unexpected("SVC001", "Service unavailable.") };
        var result = Result<string>.Failure(errors);

        // Act
        cut.Instance.HandleResult(result, fieldSelector: e => "Status");

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Status")).ToList();
        Assert.Contains("Service unavailable.", messages);

        var generalMessages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.IsEmpty(generalMessages);
    }

    [TestMethod]
    public void HandleResult_WithFieldSelectorReturningEmpty_MapsToGeneralErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[] { Error.Validation("Name", "Name is required.") };
        var result = Result<string>.Failure(errors);

        // Act
        cut.Instance.HandleResult(result, fieldSelector: _ => string.Empty);

        // Assert
        var generalMessages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("Name is required.", generalMessages);

        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(nameMessages);
    }
}
