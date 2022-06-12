//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace D20Tek.BlazorComponents
{
    internal class ToggleSizeMetadata
    {
        private static IDictionary<Size, string> _elements = new Dictionary<Size, string>
        {
            { Size.None, "toggle-md" },
            { Size.ExtraSmall, "toggle-xs" },
            { Size.Small, "toggle-sm" },
            { Size.Medium, "toggle-md" },
            { Size.Large, "toggle-lg" },
            { Size.ExtraLarge, "toggle-xl" },
        };

        public static string GetSizeCss(Size size) => _elements[size];
    }
}
