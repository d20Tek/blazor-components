namespace D20Tek.BlazorComponents;

public partial class SpanTimer : RadialTimer
{
    private int _timerDuration = 30;
    private TimeSpan _timerDurationSpan = new(0, 0, 30);

    public override int TimerDuration
    {
        get => _timerDuration;
        set
        {
            _validTimeRange.AssertInRange(value, nameof(TimerDuration));
            _timerDuration = value;
        }
    }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Needed")]
    public TimeSpan TimerDurationSpan
    {
        get => _timerDurationSpan;
        set
        {
            TimerDuration = (int)value.TotalSeconds;
            _timerDurationSpan = value;
        }
    }
}
