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
        mapper.Map([Error.Validation("Name", "req")], selector, UnknownFieldBehavior.PassThrough);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_KnownFieldsNull_PassesThroughRegardlessOfBehavior()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");

        // act
        mapper.Map([Error.Validation("Name", "req")], selector, UnknownFieldBehavior.SendToSummary);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("UnknownField")).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_SelectorReturnsFieldName_AddsErrorToNamedField()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("Name");

        // act
        mapper.Map([Error.Validation("Name", "req")], selector, UnknownFieldBehavior.PassThrough);

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
        mapper.Map([Error.Validation("X", "err")], selector, UnknownFieldBehavior.PassThrough);

        // assert
        Assert.Contains("err", [.. editContext.GetValidationMessages(editContext.Field("A"))]);
        Assert.Contains("err", [.. editContext.GetValidationMessages(editContext.Field("B"))]);
    }

    [TestMethod]
    public void Map_DropBehaviorWithUnknownField_SkipsErrorEntirely()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.Drop, knownFields);

        // assert
        var unknown = editContext.GetValidationMessages(editContext.Field("UnknownField")).ToList();
        var summary = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        var known = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.IsEmpty(unknown);
        Assert.IsEmpty(summary);
        Assert.IsEmpty(known);
    }

    [TestMethod]
    public void Map_KnownFieldsEmpty_PassesThroughRawFieldName()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("AnyField");
        var knownFields = new HashSet<string>();

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.Drop, knownFields);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("AnyField")).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_EmptyRawFieldName_PassesThroughToFormLevel()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector(string.Empty);
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.Drop, knownFields);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_KnownFieldMatch_AddsErrorToNamedField()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("Name");
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.Drop, knownFields);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("Name")).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_PassThroughWithUnknownField_AddsErrorToUnknownField()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.PassThrough, knownFields);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("UnknownField")).ToList();
        Assert.Contains("req", messages);
    }

    [TestMethod]
    public void Map_SendToSummaryWithUnknownField_AddsErrorToFormLevel()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.SendToSummary, knownFields);

        // assert
        var summary = editContext.GetValidationMessages(editContext.Field(string.Empty)).ToList();
        var unknown = editContext.GetValidationMessages(editContext.Field("UnknownField")).ToList();
        Assert.Contains("req", summary);
        Assert.IsEmpty(unknown);
    }

    [TestMethod]
    public void Map_ThrowWithUnknownField_ThrowsInvalidOperationException()
    {
        // arrange
        var (mapper, _, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");
        var knownFields = new HashSet<string> { "Name" };

        // act-assert
        var ex = Assert.ThrowsExactly<InvalidOperationException>([ExcludeFromCodeCoverage]() =>
            mapper.Map([Error.Validation("X", "req")], selector, UnknownFieldBehavior.Throw, knownFields));
        Assert.Contains("UnknownField", ex.Message);
        Assert.Contains("X", ex.Message);
    }

    [TestMethod]
    public void Map_InvalidBehaviorEnumValue_FallsBackToPassThrough()
    {
        // arrange
        var (mapper, editContext, _) = CreateMapper();
        var selector = new ConstantSelector("UnknownField");
        var knownFields = new HashSet<string> { "Name" };

        // act
        mapper.Map([Error.Validation("X", "req")], selector, (UnknownFieldBehavior)999, knownFields);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("UnknownField")).ToList();
        Assert.Contains("req", messages);
    }
}
