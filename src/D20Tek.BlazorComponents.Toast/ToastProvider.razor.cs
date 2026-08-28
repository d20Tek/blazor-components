namespace D20Tek.BlazorComponents;

public partial class ToastProvider : ComponentBase, IDisposable
{
    private readonly List<ToastInstance> _toasts = [];
    private readonly ConcurrentDictionary<Guid, Timer> _timers = new();
    private bool _disposed;

    [Parameter] public int MaxVisible { get; set; } = 5;

    [Parameter] public string CloseButtonAriaLabel { get; set; } = "Dismiss";

    protected override void OnInitialized()
    {
        ToastService.OnShow += HandleShow;
        ToastService.OnDismiss += HandleDismiss;
    }

    private void HandleShow(ToastInstance toast)
    {
        lock (_toasts)
        {
            _toasts.Add(toast);
        }

        if (!toast.IsSticky) StartTimer(toast);

        InvokeAsync(StateHasChanged);
    }

    private void HandleDismiss(Guid id) => RemoveToast(id);

    private void Dismiss(Guid id) => RemoveToast(id);

    private void RemoveToast(Guid id)
    {
        if (_timers.TryRemove(id, out var timer)) timer.Dispose();

        lock (_toasts)
        {
            _toasts.RemoveAll(t => t.Id == id);
        }

        InvokeAsync(StateHasChanged);
    }

    private void StartTimer(ToastInstance toast) =>
        _timers[toast.Id] = new Timer(
            _ => RemoveToast(toast.Id),
            null,
            toast.Timeout,
            Timeout.InfiniteTimeSpan);

    private IEnumerable<ToastGroup> GetVisibleGroups()
    {
        List<ToastInstance> snapshot;
        lock (_toasts)
        {
            snapshot = [.. _toasts];
        }

        return snapshot.GroupBy(t => t.Position)
                       .Select(g => new ToastGroup(
                            g.Key,
                            [.. g.OrderBy(t => t.CreatedAt).Take(MaxVisible)]));
    }

    private static string? GetToastCssClass(ToastInstance toast) =>
        new CssBuilder("d20tek-toast")
            .AddClass($"d20tek-toast-{NotificationVariantMetadata.GetVariantToken(toast.Variant)}")
            .AddClass("d20tek-toast-animated", toast.Animate)
            .AddClass("d20tek-toast-dismissible", toast.Dismissible)
            .Build();

    private static RenderFragment DefaultIcon(ToastInstance toast) => builder =>
        builder.AddMarkupContent(0, NotificationVariantMetadata.GetDefaultIcon(toast.Variant));

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        ToastService.OnShow -= HandleShow;
        ToastService.OnDismiss -= HandleDismiss;

        foreach (var timer in _timers.Values)
        {
            timer.Dispose();
        }

        _timers.Clear();
        GC.SuppressFinalize(this);
    }

    private sealed record ToastGroup(ToastPosition Key, IReadOnlyList<ToastInstance> Items);
}
