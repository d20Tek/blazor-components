namespace D20Tek.BlazorComponents.Selectors;

public sealed class CompositeFieldSelector : IErrorFieldSelector
{
    private readonly IErrorFieldSelector[] _selectors;

    public CompositeFieldSelector(params IErrorFieldSelector[] selectors)
    {
        ArgumentNullException.ThrowIfNull(selectors);
        if (selectors.Length == 0)
            throw new ArgumentException("At least one selector must be provided.", nameof(selectors));
        _selectors = selectors;
    }

    public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context)
    {
        foreach (var selector in _selectors)
        {
            var produced = false;
            foreach (var name in selector.GetFieldNames(error, context))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    produced = true;
                    yield return name;
                }
            }
            if (produced) yield break;
        }

        yield return IErrorFieldSelector.FormLevelField;
    }
}
