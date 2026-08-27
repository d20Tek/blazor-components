namespace D20Tek.BlazorComponents.Selectors;

public sealed class CodeAsFieldSelector : IErrorFieldSelector
{
    public static readonly CodeAsFieldSelector Instance = new();

    public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context)
    {
        yield return error.Type is ErrorType.Validation
            ? error.Code
            : IErrorFieldSelector.FormLevelField;
    }
}
