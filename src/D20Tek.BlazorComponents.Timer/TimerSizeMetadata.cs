//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace D20Tek.BlazorComponents
{
    internal class TimerSizeMetadata
    {
        private static IDictionary<Size, string> _elements = new Dictionary<Size, string>
        {
            { Size.None, String.Empty },
            { Size.ExtraSmall, "base-timer-xs" },
            { Size.Small, "base-timer-sm" },
            { Size.Medium, "base-timer-md" },
            { Size.Large, "base-timer-lg" },
            { Size.ExtraLarge, "base-timer-xl" },
        };

        public static string GetSizeCss(Size size) => _elements[size];
    }
}
