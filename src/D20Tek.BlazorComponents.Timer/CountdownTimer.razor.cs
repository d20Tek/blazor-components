namespace D20Tek.BlazorComponents;

public partial class CountdownTimer : TimerBase
{
    private const string _cssTimerMain = "base-timer";

    [Parameter]
    public DateTimeOffset CountdownTarget { get; set; } = DateTimeOffset.Now.AddDays(1);

    [Parameter]
    public string? LabelText { get; set; } = null;

    public string TimerDisplay { get; private set; } = "...";

    private bool HasLabelText => !string.IsNullOrWhiteSpace(LabelText);

    public CountdownTimer() => Size = Size.None;

    protected override string? CalculateCssClasses() =>
        new CssBuilder(_cssTimerMain).AddClass(TimerSizeMetadata.GetSizeCss(Size))
                                     .AddClassFromAttributes(RemainingAttributes)
                                     .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder().AddStyleFromAttributes(RemainingAttributes)
                          .Build();

    protected override int ProcessTimerChange() => ProcessTimerDelta(CountdownTarget - DateTimeOffset.Now);

    private int ProcessTimerDelta(TimeSpan delta)
    {
        TimerDisplay = TimeDisplayFormatter.FormatTimeSpanRemaining(delta, ExpirationMessage);
        return (int)Math.Floor(delta.TotalSeconds);
    }
}
