using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class JsonPropertyNameFieldSelectorTests
{
    private sealed class Model
    {
        [JsonPropertyName("full_name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email_address")]
        public string Email { get; set; } = string.Empty;

        public string Untagged { get; set; } = string.Empty;
    }

    private static FieldSelectorContext ContextFor<T>() where T : new()
    {
        var model = new T();
        return new FieldSelectorContext(new EditContext(model), typeof(T));
    }

    [TestMethod]
    public void GetFieldNames_ValidationErrorMatchingJsonName_ReturnsPropertyName()
    {
        // arrange
        var sut = new JsonPropertyNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("full_name", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_MatchingJsonName_IsCaseInsensitive()
    {
        // arrange
        var sut = new JsonPropertyNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("EMAIL_ADDRESS", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Email"], result);
    }

    [TestMethod]
    public void GetFieldNames_NoJsonNameMatch_ReturnsErrorCodeAsFallback()
    {
        // arrange
        var sut = new JsonPropertyNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Validation("Unknown", "req"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Unknown"], result);
    }

    [TestMethod]
    public void GetFieldNames_NonValidationError_ReturnsFormLevelField()
    {
        // arrange
        var sut = new JsonPropertyNameFieldSelector();

        // act
        var result = sut.GetFieldNames(Error.Unexpected("full_name", "boom"), ContextFor<Model>()).ToArray();

        // assert
        Assert.AreSequenceEqual([IErrorFieldSelector.FormLevelField], result);
    }

    [TestMethod]
    public void GetFieldNames_NullModelType_FallsBackToErrorCode()
    {
        // arrange
        var sut = new JsonPropertyNameFieldSelector();
        var context = new FieldSelectorContext(new EditContext(new object()), ModelType: null);

        // act
        var result = sut.GetFieldNames(Error.Validation("full_name", "req"), context).ToArray();

        // assert
        Assert.AreSequenceEqual(["full_name"], result);
    }
}
