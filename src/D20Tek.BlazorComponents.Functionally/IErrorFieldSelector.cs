namespace D20Tek.BlazorComponents;

public interface IErrorFieldSelector
{
    public const string FormLevelField = "";

    IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context);
}
