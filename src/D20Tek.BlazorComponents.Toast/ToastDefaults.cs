namespace D20Tek.BlazorComponents;

public sealed class ToastDefaults
{
    public ToastPosition Position { get; set; } = ToastPosition.BottomCenter;

    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(3);

    public bool ShowIcon { get; set; } = true;

    public bool Dismissible { get; set; } = true;

    public bool Animate { get; set; } = true;
}
