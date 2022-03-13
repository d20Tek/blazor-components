//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents
{
    internal static class TimeDisplayFormatter
    {
        private const int _secondsPerMin = 60;
        private const int _secondsPerHour = _secondsPerMin * 60;

        public static string FormatTimeRemaining(int time, string expirationMessage)
        {
            expirationMessage.ThrowWhenEmpty(nameof(expirationMessage));

            if (time <= 0)
            {
                return expirationMessage;
            }

            int hours = (int)Math.Floor((decimal)time / _secondsPerHour);
            int remainingTime = time % _secondsPerHour;
            int minutes = (int)Math.Floor((decimal)remainingTime / _secondsPerMin);
            int seconds = remainingTime % _secondsPerMin;

            return FormatTimeRemaining(hours, minutes, seconds);
        }

        public static string FormatTimeRemaining(int hours, int minutes, int seconds)
        {
            if (hours > 0)
            {
                // The output in HH:MM:SS format
                return $"{hours}:{minutes:D2}:{seconds:D2}";
            }

            // The output in MM:SS format
            return $"{minutes}:{seconds:D2}";
        }
    }
}
