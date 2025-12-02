using D20Tek.BlazorComponents;
using Microsoft.AspNetCore.Components;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class TimerPage
{
    private BlazorComponents.Timer? _ref;
    private int _interactiveDuration = 30;
    private string _interactiveExpirationMessage = "Done!";
    private int _interactiveWarningThreshold = 15;
    private int _interactiveAlertThreshold = 8;
    private string _interactiveRemainingColor = "green";
    private string _interactiveWarningColor = "orange";
    private string _interactiveAlertColor = "red";
    private string _interactiveElapsedColor = "gray";
    private Size _interactiveSize = Size.Medium;
    private bool _interactiveVisibility = true;
    private bool _timerVisible = true;

    private string ToggleLabel => _interactiveVisibility ? "(visible)" : "(hidden)";

    private void ToggleTimerVisibility() => _timerVisible = !_timerVisible;

    private void OnDurationChanged(ChangeEventArgs args)
    {
        string change = args.Value?.ToString() ?? "none";
        switch (change)
        {
            case "10s":
                ResetTimerIfNeeded(10);
                break;
            case "30s":
                ResetTimerIfNeeded(30);
                break;
            case "1m":
                ResetTimerIfNeeded(60);
                break;
            case "2m":
                ResetTimerIfNeeded(120);
                break;
            case "5m":
                ResetTimerIfNeeded(300);
                break;
        }
    }

    private void ResetTimerIfNeeded(int newDuration)
    {
        _interactiveDuration = newDuration;
        _ref?.ResetTimer();
    }
}
