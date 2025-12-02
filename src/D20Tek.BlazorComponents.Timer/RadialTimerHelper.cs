namespace D20Tek.BlazorComponents;

internal static class RadialTimerHelper
{
    public static (int Counter, int Remaining) UpdateTimerRemaining(this RadialTimer timer, int timeCounter)
    {
        var next = timeCounter + 1;
        var remaining = Math.Max(0, timer.TimerDuration - next);
        var clampedCounter = remaining == 0 ? timer.TimerDuration : next;
        
        return (clampedCounter, remaining);
    }

    public static double CalculateTimeFraction(this RadialTimer timer)
    {
        var duration = (double)timer.TimerDuration;
        var rawFraction = timer.TimeRemaining / duration;
        return rawFraction - (1 / duration) * (1 - rawFraction);
    }

    public static string GetRemainingPathColor(this RadialTimer timer, int timeLeft) =>
        timeLeft switch
        {
            var t when t <= timer.AlertThreshold => timer.AlertTimeColor,
            var t when t <= timer.WarningThreshold => timer.WarningTimeColor,
            _ => timer.RemainingTimeColor
        };

}
