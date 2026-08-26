using Microsoft.AspNetCore.Components.Forms;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public sealed class ResultValidatorTests : BunitContext
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    private (IRenderedComponent<ResultValidator> validator, EditContext editContext) RenderValidator()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);

        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        return (cut, editContext);
    }

    [TestMethod]
    public void HandleResult_SuccessWithoutOnSuccess_ReturnsTrue()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var result = Result<string>.Success("value");

        // Act
        var success = cut.Instance.HandleResult(result);

        // Assert
        Assert.IsTrue(success);
    }

    [TestMethod]
    public void HandleResult_SuccessWithOnSuccess_InvokesActionAndReturnsTrue()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var result = Result<string>.Success("test-value");
        string? captured = null;

        // Act
        var success = cut.Instance.HandleResult(result, v => captured = v);

        // Assert
        Assert.IsTrue(success);
        Assert.AreEqual("test-value", captured);
    }

    [TestMethod]
    public void HandleResult_FailureWithValidationErrors_ReturnsFalseAndMapsFieldErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[]
        {
            Error.Validation("Name", "Name is required."),
            Error.Validation("Age", "Age must be positive.")
        };
        var result = Result<string>.Failure(errors);

        // Act
        var success = cut.Instance.HandleResult(result);

        // Assert
        Assert.IsFalse(success);

        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Name is required.", nameMessages);

        var ageMessages = editContext.GetValidationMessages(editContext.Field("Age")).ToList();
        Assert.Contains("Age must be positive.", ageMessages);
    }

    [TestMethod]
    public void HandleResult_FailureWithNonValidationError_MapsToEmptyField()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[]
        {
            Error.Unexpected("GeneralError", "Something went wrong.")
        };
        var result = Result<string>.Failure(errors);

        // Act
        var success = cut.Instance.HandleResult(result);

        // Assert
        Assert.IsFalse(success);

        var generalMessages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("Something went wrong.", generalMessages);
    }

    [TestMethod]
    public void HandleResult_FailureWithOnSuccess_DoesNotInvokeAction()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var errors = new Error[] { Error.Validation("Name", "Required.") };
        var result = Result<string>.Failure(errors);
        var invoked = false;

        // Act
        cut.Instance.HandleResult(result, [ExcludeFromCodeCoverage](_) => invoked = UnreachableOnSuccess());

        // Assert
        Assert.IsFalse(invoked);
    }

    [ExcludeFromCodeCoverage]
    private static bool UnreachableOnSuccess() => true;

    [TestMethod]
    public void HandleResult_FailureWithMixedErrorTypes_MapsCorrectly()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[]
        {
            Error.Validation("Name", "Name is required."),
            Error.Unexpected("ServerError", "Internal failure.")
        };
        var result = Result<string>.Failure(errors);

        // Act
        cut.Instance.HandleResult(result);

        // Assert
        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Name is required.", nameMessages);

        var generalMessages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("Internal failure.", generalMessages);
    }

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

    [TestMethod]
    public async Task HandleResultAsync_SuccessWithoutOnSuccess_ReturnsTrue()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var result = Result<string>.Success("value");

        // Act
        var success = await cut.Instance.HandleResultAsync(result);

        // Assert
        Assert.IsTrue(success);
    }

    [TestMethod]
    public async Task HandleResultAsync_SuccessWithOnSuccess_InvokesAsyncActionAndReturnsTrue()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var result = Result<string>.Success("test-value");
        string? captured = null;

        // Act
        var success = await cut.Instance.HandleResultAsync(result, async v =>
        {
            await Task.Yield();
            captured = v;
        });

        // Assert
        Assert.IsTrue(success);
        Assert.AreEqual("test-value", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_FailureWithValidationErrors_ReturnsFalseAndMapsFieldErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[]
        {
            Error.Validation("Name", "Name is required."),
            Error.Validation("Age", "Age must be positive.")
        };
        var result = Result<string>.Failure(errors);

        // Act
        var success = await cut.Instance.HandleResultAsync(result);

        // Assert
        Assert.IsFalse(success);

        var nameMessages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Name is required.", nameMessages);

        var ageMessages = editContext.GetValidationMessages(editContext.Field("Age")).ToList();
        Assert.Contains("Age must be positive.", ageMessages);
    }

    [TestMethod]
    public async Task HandleResultAsync_FailureWithNonValidationError_MapsToEmptyField()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[]
        {
            Error.Unexpected("GeneralError", "Something went wrong.")
        };
        var result = Result<string>.Failure(errors);

        // Act
        var success = await cut.Instance.HandleResultAsync(result);

        // Assert
        Assert.IsFalse(success);

        var generalMessages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("Something went wrong.", generalMessages);
    }

    [TestMethod]
    public async Task HandleResultAsync_FailureWithOnSuccess_DoesNotInvokeAction()
    {
        // Arrange
        var (cut, _) = RenderValidator();
        var errors = new Error[] { Error.Validation("Name", "Required.") };
        var result = Result<string>.Failure(errors);

        // Act
        await cut.Instance.HandleResultAsync(result, UnreachableOnSuccessAsync);

        // Assert - if we reach here without exception, the callback was not invoked
        Assert.IsTrue(result.IsFailure);
    }

    [ExcludeFromCodeCoverage]
    private static Task UnreachableOnSuccessAsync(string _) =>
        throw new InvalidOperationException("This should never be called.");

    [TestMethod]
    public async Task HandleResultAsync_WithFieldSelector_MapsToCustomField()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var errors = new Error[] { Error.Validation("account_name", "Name is required.") };
        var result = Result<string>.Failure(errors);

        // Act
        await cut.Instance.HandleResultAsync(result, fieldSelector: e => "Name");

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("Name is required.", messages);
    }

    [TestMethod]
    public async Task HandleResultAsync_CalledTwice_ClearsPreviousErrors()
    {
        // Arrange
        var (cut, editContext) = RenderValidator();
        var firstResult = Result<string>.Failure([Error.Validation("Name", "First error.")]);
        var secondResult = Result<string>.Success("ok");

        await cut.Instance.HandleResultAsync(firstResult);

        // Act
        await cut.Instance.HandleResultAsync(secondResult);

        // Assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(messages);
    }
}
