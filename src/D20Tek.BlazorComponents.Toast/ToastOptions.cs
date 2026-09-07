namespace D20Tek.BlazorComponents;

public sealed class ToastOptions
{
    public NotificationVariant Variant { get; set; } = NotificationVariant.Info;

    public ToastPosition Position { get; set; } = ToastPosition.BottomCenter;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

    public bool ShowIcon { get; set; } = true;

    public bool Dismissible { get; set; } = true;

    public bool Animate { get; set; } = true;

    public bool IsSticky => Timeout <= TimeSpan.Zero;
}
