using D20Tek.BlazorComponents;
using Microsoft.AspNetCore.Components;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class CountdownTimerPage
{
    private CountdownTimer? _ref;
    private DateTimeOffset _interactiveCountdownTarget = DateTimeOffset.Now.AddHours(1);
    private string _interactiveExpirationMessage = "Done!";
    private string _interactiveLabelText = "Label text:";
    private Size _interactiveSize = Size.Medium;
    private bool _interactiveVisibility = true;
    private bool _interativeTimerNeedsReset = false;
    private bool _timerVisible = true;
    private DateTimeOffset _birthdate = new DateTimeOffset(2022, 5, 3, 8, 0, 0, new TimeSpan(-8, 0, 0));

    private string ToggleLabel => _interactiveVisibility ? "(visible)" : "(hidden)";

    private void ToggleTimerVisibility() => _timerVisible = !_timerVisible;

    private void OnTimerExpired() => _interativeTimerNeedsReset = true;

    private void OnTargetChanged(ChangeEventArgs args)
    {
        string change = args.Value?.ToString() ?? "none";
        switch (change)
        {
            case "10s":
                ResetTimerIfNeeded(DateTimeOffset.Now.AddSeconds(10));
                break;
            case "1m":
                ResetTimerIfNeeded(DateTimeOffset.Now.AddMinutes(1));
                break;
            case "5m":
                ResetTimerIfNeeded(DateTimeOffset.Now.AddMinutes(5));
                break;
            case "1hr":
                ResetTimerIfNeeded(DateTimeOffset.Now.AddHours(1));
                break;
            case "1d":
                ResetTimerIfNeeded(DateTimeOffset.Now.AddDays(1));
                break;
        }
    }

    private void ResetTimerIfNeeded(DateTimeOffset newTarget)
    {
        _interactiveCountdownTarget = newTarget;
        if (_interativeTimerNeedsReset)
        {
            _ref?.ResetTimer();
            _interativeTimerNeedsReset = false;
        }
    }
}
