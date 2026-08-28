using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents;

public sealed class ResultValidatorOptions
{
    public IErrorFieldSelector FieldSelector { get; set; } = CodeAsFieldSelector.Instance;

    public ResultValidatorOptions UseSelector(IErrorFieldSelector selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        FieldSelector = selector;
        return this;
    }

    public ResultValidatorOptions UseCodeAsField()
    {
        FieldSelector = CodeAsFieldSelector.Instance;
        return this;
    }

    public ResultValidatorOptions UseDisplayNames()
    {
        FieldSelector = new DisplayNameFieldSelector();
        return this;
    }

    public ResultValidatorOptions UseJsonPropertyNames()
    {
        FieldSelector = new JsonPropertyNameFieldSelector();
        return this;
    }

    public ResultValidatorOptions StripPrefixes(params string[] prefixes)
    {
        FieldSelector = new PrefixStrippingFieldSelector(FieldSelector, prefixes);
        return this;
    }

    public ResultValidatorOptions Compose(params IErrorFieldSelector[] additional)
    {
        FieldSelector = new CompositeFieldSelector([FieldSelector, .. additional]);
        return this;
    }
}
