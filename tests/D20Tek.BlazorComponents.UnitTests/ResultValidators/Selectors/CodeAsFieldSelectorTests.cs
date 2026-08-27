using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class CodeAsFieldSelectorTests
{
    private static FieldSelectorContext CreateContext()
    {
        var model = new object();
        return new FieldSelectorContext(new EditContext(model), model.GetType());
    }

    [TestMethod]
    public void GetFieldNames_ValidationError_ReturnsErrorCode()
    {
        // arrange
        var sut = CodeAsFieldSelector.Instance;

        // act
        var result = sut.GetFieldNames(Error.Validation("Name", "Required."), CreateContext()).ToArray();

        // assert
        Assert.HasCount(1, result);
        Assert.AreEqual("Name", result[0]);
    }

    [TestMethod]
    public void GetFieldNames_NonValidationError_ReturnsFormLevelField()
    {
        // arrange
        var sut = CodeAsFieldSelector.Instance;

        // act
        var result = sut.GetFieldNames(Error.Unexpected("SVC", "Boom."), CreateContext()).ToArray();

        // assert
        Assert.HasCount(1, result);
        Assert.AreEqual(IErrorFieldSelector.FormLevelField, result[0]);
    }

    [TestMethod]
    public void Instance_ReturnsSameInstance()
    {
        // arrange
        var a = CodeAsFieldSelector.Instance;

        // act
        var b = CodeAsFieldSelector.Instance;

        // assert
        Assert.AreSame(a, b);
    }
}
