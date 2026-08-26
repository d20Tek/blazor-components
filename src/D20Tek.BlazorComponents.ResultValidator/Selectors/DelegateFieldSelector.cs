namespace D20Tek.BlazorComponents.Selectors;

public sealed class DelegateFieldSelector : IErrorFieldSelector
{
    private readonly Func<Error, FieldSelectorContext, IEnumerable<string>> _selector;

    public DelegateFieldSelector(Func<Error, string> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = (e, _) => [selector(e)];
    }

    public DelegateFieldSelector(Func<Error, FieldSelectorContext, string> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = (e, c) => new[] { selector(e, c) };
    }

    public DelegateFieldSelector(Func<Error, FieldSelectorContext, IEnumerable<string>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        _selector = selector;
    }

    public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context) => _selector(error, context);
}
