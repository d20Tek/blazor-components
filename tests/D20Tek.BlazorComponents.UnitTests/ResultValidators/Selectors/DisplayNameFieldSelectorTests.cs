using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class DisplayNameFieldSelectorTests
{
    private sealed class Model
    {
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        public string Untagged { get; set; } = string.Empty;
    }

    private static FieldSelectorContext ContextFor<T>() where T : new()
    {
        var model = new T();
        return new FieldSelectorContext(new EditContext(model), typeof(T));
    }

    [TestMethod]
    public void GetFieldNames_ValidationErrorMatchingDisplayName_ReturnsPropertyName()
    {
        // arrange
        var sut = new DisplayNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("Full Name", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_MatchingDisplayName_IsCaseInsensitive()
    {
        // arrange
        var sut = new DisplayNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("full name", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_NoDisplayNameMatch_ReturnsErrorCodeAsFallback()
    {
        // arrange
        var sut = new DisplayNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("Unknown", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Unknown"], result);
    }

    [TestMethod]
    public void GetFieldNames_NonValidationError_ReturnsFormLevelField()
    {
        // arrange
        var sut = new DisplayNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Unexpected("Full Name", "boom"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual([IErrorFieldSelector.FormLevelField], result);
    }

    [TestMethod]
    public void GetFieldNames_NullModelType_FallsBackToErrorCode()
    {
        // arrange
        var sut = new DisplayNameFieldSelector();
        var context = new FieldSelectorContext(new EditContext(new object()), ModelType: null);

        // act
        var result = sut.GetFieldNames(Error.Validation("Full Name", "req"), context).ToArray();

        // assert
        Assert.AreSequenceEqual(["Full Name"], result);
    }
}
