namespace D20Tek.BlazorComponents;

public interface IToastService
{
    event Action<ToastInstance>? OnShow;

    event Action<Guid>? OnDismiss;

    ToastDefaults Defaults { get; }

    ToastInstance Show(RenderFragment content, Action<ToastOptions>? configure = null);

    ToastInstance Show(string message, Action<ToastOptions>? configure = null);

    ToastInstance Show(string message, NotificationVariant variant, Action<ToastOptions>? configure = null);

    void Dismiss(Guid id);
}
