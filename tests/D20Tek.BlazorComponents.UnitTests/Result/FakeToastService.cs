namespace D20Tek.BlazorComponents.UnitTests.Result;

[ExcludeFromCodeCoverage]
public sealed class FakeToastService : IToastService
{
    public event Action<ToastInstance>? OnShow;
    public event Action<Guid>? OnDismiss;

    public ToastInstance? LastToast { get; private set; }

    public ToastInstance Show(RenderFragment content, Action<ToastOptions>? configure = null)
    {
        var options = new ToastOptions();
        configure?.Invoke(options);
        var toast = new ToastInstance
        {
            Content = content,
            Variant = options.Variant,
            Position = options.Position,
            Timeout = options.Timeout,
            ShowIcon = options.ShowIcon,
            Dismissible = options.Dismissible,
            Animate = options.Animate,
        };
        LastToast = toast;
        OnShow?.Invoke(toast);
        return toast;
    }

    public ToastInstance Show(string message, Action<ToastOptions>? configure = null) =>
        Show((RenderFragment)(builder => builder.AddContent(0, message)), configure);

    public ToastInstance Show(
        string message,
        NotificationVariant variant,
        Action<ToastOptions>? configure = null) =>
        Show(message, o => { o.Variant = variant; configure?.Invoke(o); });

    public void Dismiss(Guid id) => OnDismiss?.Invoke(id);
}
