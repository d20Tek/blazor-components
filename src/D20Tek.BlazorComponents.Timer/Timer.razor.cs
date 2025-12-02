namespace D20Tek.BlazorComponents;

public partial class Timer : RadialTimer
{
    private int _timerDuration = 30;

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Needed")]
    public override int TimerDuration
    {
        get => _timerDuration;
        set
        {
            _validTimeRange.AssertInRange(value, nameof(TimerDuration));
            _timerDuration = value;
        }
    }

    public Timer() { }
}
