namespace D20Tek.BlazorComponents;

public sealed class ToastInstance
{
    public Guid Id { get; } = Guid.NewGuid();

    public required RenderFragment Content { get; init; }

    public NotificationVariant Variant { get; init; } = NotificationVariant.Info;

    public ToastPosition Position { get; init; } = ToastPosition.BottomRight;

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(5);

    public bool ShowIcon { get; init; } = true;

    public bool Dismissible { get; init; } = true;

    public bool Animate { get; init; } = true;

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;

    public bool IsSticky => Timeout <= TimeSpan.Zero;
}
