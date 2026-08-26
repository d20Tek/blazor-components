using D20Tek.BlazorComponents.Selectors;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public sealed class ResultValidatorSelectorResolutionTests : BunitContext
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class ConstantSelector(string field) : IErrorFieldSelector
    {
        public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context) => [field];
    }

    [TestMethod]
    public void FieldSelectorParameter_OverridesDefaultSelector()
    {
        // arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var custom = new ConstantSelector("CustomField");
        var cut = Render<ResultValidator>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.FieldSelector, custom));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("CustomField")).ToList();
        Assert.Contains("Required.", messages);
    }

    [TestMethod]
    public void PerCallSelector_OverridesFieldSelectorParameter()
    {
        // arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var parameterSelector = new ConstantSelector("ParameterField");
        var cut = Render<ResultValidator>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.FieldSelector, parameterSelector));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result, fieldSelector: _ => "PerCallField");

        // assert
        var perCall = editContext.GetValidationMessages(editContext.Field("PerCallField")).ToList();
        Assert.Contains("Required.", perCall);

        var parameter = editContext.GetValidationMessages(editContext.Field("ParameterField")).ToList();
        Assert.IsEmpty(parameter);
    }

    [TestMethod]
    public void InjectedSelector_UsedAsDefaultWhenNoParameter()
    {
        // arrange
        Services.AddResultValidator(o => o.UseSelector(new ConstantSelector("InjectedField")));
        var model = new TestModel();
        var editContext = new EditContext(model);
        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result);

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("InjectedField")).ToList();
        Assert.Contains("Required.", messages);
    }

    [TestMethod]
    public void FieldSelectorParameter_OverridesInjectedSelector()
    {
        // arrange
        Services.AddResultValidator(o => o.UseSelector(new ConstantSelector("InjectedField")));
        var model = new TestModel();
        var editContext = new EditContext(model);
        var custom = new ConstantSelector("ParameterField");
        var cut = Render<ResultValidator>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.FieldSelector, custom));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result);

        // assert
        var parameter = editContext.GetValidationMessages(editContext.Field("ParameterField")).ToList();
        Assert.Contains("Required.", parameter);

        var injected = editContext.GetValidationMessages(editContext.Field("InjectedField")).ToList();
        Assert.IsEmpty(injected);
    }

    [TestMethod]
    public void HandleResult_WithSelectorInterfaceOverload_UsesProvidedSelector()
    {
        // arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result, onSuccess: null, new ConstantSelector("OverloadField"));

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("OverloadField")).ToList();
        Assert.Contains("Required.", messages);
    }

    [TestMethod]
    public async Task HandleResultAsync_WithSelectorInterfaceOverload_UsesProvidedSelector()
    {
        // arrange
        var model = new TestModel();
        var editContext = new EditContext(model);
        var cut = Render<ResultValidator>(parameters =>
            parameters.AddCascadingValue(editContext));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        await cut.Instance.HandleResultAsync(result, onSuccess: null, new ConstantSelector("OverloadField"));

        // assert
        var messages = editContext.GetValidationMessages(editContext.Field("OverloadField")).ToList();
        Assert.Contains("Required.", messages);
    }

    [TestMethod]
    public void SelectorReturningMultipleFieldNames_AddsErrorToAllFields()
    {
        // arrange
        var multiField = new DelegateFieldSelector((e, _) => [ "FieldA", "FieldB" ]);
        var model = new TestModel();
        var editContext = new EditContext(model);
        var cut = Render<ResultValidator>(parameters => parameters
            .AddCascadingValue(editContext)
            .Add(p => p.FieldSelector, multiField));

        // act
        var result = Result<string>.Failure([Error.Validation("Name", "Required.")]);
        cut.Instance.HandleResult(result);

        // assert
        Assert.Contains("Required.", [.. editContext.GetValidationMessages(editContext.Field("FieldA"))]);
        Assert.Contains("Required.", [.. editContext.GetValidationMessages(editContext.Field("FieldB"))]);
    }
}
