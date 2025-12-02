namespace D20Tek.BlazorComponents;

internal static class TimeDisplayFormatter
{
    private const int _secondsPerMin = 60;
    private const int _secondsPerHour = _secondsPerMin * 60;

    public static string FormatTimeRemaining(int time, string expirationMessage)
    {
        expirationMessage.ThrowWhenEmpty(nameof(expirationMessage));

        if (time <= 0) return expirationMessage;

        int hours = (int)Math.Floor((decimal)time / _secondsPerHour);
        int remainingTime = time % _secondsPerHour;
        int minutes = (int)Math.Floor((decimal)remainingTime / _secondsPerMin);
        int seconds = remainingTime % _secondsPerMin;

        return FormatTimeRemaining(hours, minutes, seconds);
    }

    public static string FormatTimeRemaining(int hours, int minutes, int seconds) =>
        (hours > 0)
            ? $"{hours}:{minutes:D2}:{seconds:D2}"     // The output in HH:MM:SS format
            : $"{minutes}:{seconds:D2}";               // The output in MM:SS format

    public static string FormatTimeRemaining(int days, int hours, int minutes, int seconds) =>
        (days > 0)
            ? $"{days}D {hours}:{minutes:D2}:{seconds:D2}"      // The output in HH:MM:SS format
            : $"{hours}:{minutes:D2}:{seconds:D2}";            // The output in HH:MM:SS format

    public static string FormatTimeSpanRemaining(TimeSpan time, string expirationMessage)
    {
        expirationMessage.ThrowWhenEmpty(nameof(expirationMessage));
        return (Math.Floor(time.TotalSeconds) <= 0)
            ? expirationMessage
            : FormatTimeRemaining(time.Days, time.Hours, time.Minutes, time.Seconds);
    }
}
