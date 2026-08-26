namespace D20Tek.BlazorComponents;

public sealed record FieldSelectorContext(
    EditContext EditContext,
    Type? ModelType = null);
