namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public sealed partial class ResultValidatorTests : BunitContext
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
}
