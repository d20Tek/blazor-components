using D20Tek.BlazorComponents.Selectors;
using Microsoft.AspNetCore.Components.Forms;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class ErrorMapperTests
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class ConstantSelector(params string[] names) : IErrorFieldSelector
    {
        public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context) => names;
    }

    private static (ErrorMapper mapper, EditContext editContext, ValidationMessageStore store) CreateMapper()
    {
        var model = new TestModel();
        var editContext = new EditContext(model);
        var store = new ValidationMessageStore(editContext);
        var mapper = new ErrorMapper(store, editContext);
        return (mapper, editContext, store);
    }

    [TestMethod]
    public void Map_SelectorYieldsNothing_AddsErrorToFormLevelField()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector();

        // act
        mapper.Map([Error.Validation("Name", "req")], selector);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_SelectorReturnsFieldName_AddsErrorToNamedField()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("Name");

        // act
        mapper.Map([Error.Validation("Name", "req")], selector);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_MultipleFieldNames_AddsErrorToEach()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("A", "B");

        // act
        mapper.Map([Error.Validation("X", "err")], selector);

        // assert
        Assert.Contains("err", [.. editContext.GetValidationMessages(editContext.Field("A"))]);
        Assert.Contains("err", [.. editContext.GetValidationMessages(editContext.Field("B"))]);
    }
}
