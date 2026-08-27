namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

public sealed partial class ResultValidatorTests
{
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
