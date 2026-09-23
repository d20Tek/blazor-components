namespace D20Tek.BlazorComponents;

public sealed record MenuHeaderItem : IMenuEntry
{
    public string Label { get; init; } = string.Empty;
}
