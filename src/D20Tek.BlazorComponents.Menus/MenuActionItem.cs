namespace D20Tek.BlazorComponents;

public sealed record MenuActionItem : MenuInteractiveItem
{
    public EventCallback OnClick { get; init; }
}
