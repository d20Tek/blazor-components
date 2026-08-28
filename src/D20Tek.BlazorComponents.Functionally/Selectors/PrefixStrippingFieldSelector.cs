namespace D20Tek.BlazorComponents.Selectors;

public sealed class PrefixStrippingFieldSelector : IErrorFieldSelector
{
    private readonly IErrorFieldSelector _inner;
    private readonly string[] _prefixes;
    private readonly StringComparison _comparison;

    public PrefixStrippingFieldSelector(
        IErrorFieldSelector inner,
        IEnumerable<string> prefixes,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(prefixes);
        _inner = inner;
        _prefixes = [.. prefixes];
        _comparison = comparison;
    }

    public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context)
    {
        foreach (var name in _inner.GetFieldNames(error, context))
        {
            yield return Strip(name);
        }
    }

    private string Strip(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        foreach (var prefix in _prefixes)
        {
            if (name.StartsWith(prefix, _comparison))
            {
                return name[prefix.Length..];
            }
        }
        return name;
    }
}
