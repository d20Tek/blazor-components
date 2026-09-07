namespace D20Tek.BlazorComponents;

internal sealed class ToastService : IToastService
{
    public event Action<ToastInstance>? OnShow;

    public event Action<Guid>? OnDismiss;

    public ToastDefaults Defaults { get; }

    public ToastService(ToastDefaults? defaults = null) => Defaults = defaults ?? new ToastDefaults();

    public ToastInstance Show(RenderFragment content, Action<ToastOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(content);

        var options = CreateOptions();
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

        OnShow?.Invoke(toast);
        return toast;
    }

    public ToastInstance Show(string message, Action<ToastOptions>? configure = null) =>
        Show(BuildMessageFragment(message), configure);

    public ToastInstance Show(
        string message,
        NotificationVariant variant,
        Action<ToastOptions>? configure = null) =>
        Show(BuildMessageFragment(message), options =>
        {
            options.Variant = variant;
            configure?.Invoke(options);
        });

    public void Dismiss(Guid id) => OnDismiss?.Invoke(id);

    private ToastOptions CreateOptions() => new()
    {
        Position = Defaults.Position,
        Timeout = Defaults.DefaultTimeout,
        ShowIcon = Defaults.ShowIcon,
        Dismissible = Defaults.Dismissible,
        Animate = Defaults.Animate,
    };

    private static RenderFragment BuildMessageFragment(string message) => builder => builder.AddContent(0, message);
}
