namespace D20Tek.BlazorComponents;

public abstract record MenuInteractiveItem : IMenuEntry
{
    public string Label { get; init; } = string.Empty;

    public string? Icon { get; init; }

    public RenderFragment? IconContent { get; init; }

    public bool IsDisabled { get; init; }

    public bool IsDestructive { get; init; }

    public bool CloseOnClick { get; init; } = true;

    public string? AriaLabel { get; init; }
}
