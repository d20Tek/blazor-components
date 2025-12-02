namespace D20Tek.BlazorComponents;

public abstract class RadialTimer : TimerBase
{
    private const int _fullDashArray = 283;
    private const string _cssTimerMain = "base-timer";
    protected static readonly ValueRange _validTimeRange = new(0, 1000000);

    private int _timeCounter = 0;
    private int _warningThreshold = 15;
    private int _alertThreshold = 8;

    protected string TimerElapsedColorCss => $"stroke: {ElapsedTimeColor}";

    protected string TimerPathColorCss => $"stroke: {this.GetRemainingPathColor(TimeRemaining)}";

    protected string TimerPathDashArray => 
        $"{Math.Ceiling(this.CalculateTimeFraction() * _fullDashArray)} {_fullDashArray}";

    public abstract int TimerDuration { get; set; }

    public string TimerDurationDisplay => TimeDisplayFormatter.FormatTimeRemaining(TimeRemaining, ExpirationMessage);

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Needed")]
    public int WarningThreshold
    {
        get => _warningThreshold;
        set
        {
            _validTimeRange.AssertInRange(value, nameof(WarningThreshold));
            _warningThreshold = value;
        }
    }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Needed")]
    public int AlertThreshold
    {
        get => _alertThreshold;
        set
        {
            _validTimeRange.AssertInRange(value, nameof(AlertThreshold));
            _alertThreshold = value;
        }
    }

    [Parameter]
    public string RemainingTimeColor { get; set; } = "green";

    [Parameter]
    public string WarningTimeColor { get; set; } = "orange";

    [Parameter]
    public string AlertTimeColor { get; set; } = "red";

    [Parameter]
    public string ElapsedTimeColor { get; set; } = "gray";

    protected override void OnInitialized()
    {
        TimeRemaining = TimerDuration;
        base.OnInitialized();
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder(_cssTimerMain).AddClass(TimerSizeMetadata.GetSizeCss(Size))
                                     .AddClassFromAttributes(RemainingAttributes)
                                     .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder().AddStyleFromAttributes(RemainingAttributes)
                          .Build();

    protected override void InitializeTime()
    {
        _timeCounter = 0;
        TimeRemaining = TimerDuration;
    }

    protected override int ProcessTimerChange()
    {
        (_timeCounter, TimeRemaining) = this.UpdateTimerRemaining(_timeCounter);
        return TimeRemaining;
    }
}
