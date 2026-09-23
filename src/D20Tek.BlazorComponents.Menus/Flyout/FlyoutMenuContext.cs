namespace D20Tek.BlazorComponents;

public sealed class FlyoutMenuContext(bool isOpen, EventCallback toggle)
{
    public bool IsOpen { get; } = isOpen;

    public EventCallback Toggle { get; } = toggle;
}
