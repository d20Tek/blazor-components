using Sys = System.Threading;

namespace D20Tek.BlazorComponents;

public abstract class TimerBase : BaseComponent, IDisposable
{
    private const int _millisecondsPerSec = 1000;

    private Sys.Timer? _timer;

    [Parameter]
    public EventCallback TimerExpired { get; set; }

    [Parameter]
    public string ExpirationMessage { get; set; } = "Time's up!";

    public int TimeRemaining { get; protected set; } = 0;

    public bool IsDisposed { get; private set; }

    public TimerBase() => Size = Size.Medium;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ResetTimer();
        InitializeTime();
        _timer = new Sys.Timer(OnTimerChanged, null, _millisecondsPerSec, _millisecondsPerSec);
    }

    public void ResetTimer()
    {
        InitializeTime();
        InvokeAsync(StateHasChanged);

        _timer?.Change(_millisecondsPerSec, _millisecondsPerSec);
    }

    protected virtual void InitializeTime() { }

    protected abstract int ProcessTimerChange();

    internal void OnTimerChanged(object? state)
    {
        if (ProcessTimerChange() <= 0)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            TimerExpired.InvokeAsync();
        }

        InvokeAsync(StateHasChanged);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                _timer?.Dispose();
                _timer = null;
            }

            IsDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
