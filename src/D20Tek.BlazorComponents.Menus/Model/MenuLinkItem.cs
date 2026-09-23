namespace D20Tek.BlazorComponents;

public sealed record MenuLinkItem : MenuInteractiveItem
{
    public string Href { get; init; } = string.Empty;

    public string? Target { get; init; }
}
