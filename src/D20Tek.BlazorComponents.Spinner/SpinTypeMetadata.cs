//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace D20Tek.BlazorComponents
{
    internal static class SpinTypeMetadata
    {
        internal struct Item
        {
            public SpinType Type;
            public string FixedCssClass = String.Empty;
            public int InnerDivCount = 0;
        }

        private const string _fixedSpinCssClass = "spinner";
        private const string _fixedPulseCssClass = "spinner-pulse";
        private const string _fixedSquareCssClass = "spinner-square";
        private const string _fixedHourGlassCssClass = "spinner-hourglass";
        private const string _fixedDualRingCssClass = "spinner-dualring";
        private const string _fixedSpinIosCssClass = "spinner-ios";

        private static List<Item> _elements = new List<Item>
        {
            new (){ Type = SpinType.Ring, FixedCssClass = _fixedSpinCssClass },
            new (){ Type = SpinType.Pulse, FixedCssClass = _fixedPulseCssClass },
            new (){ Type = SpinType.Square, FixedCssClass = _fixedSquareCssClass },
            new (){ Type = SpinType.Hourglass, FixedCssClass = _fixedHourGlassCssClass },
            new (){ Type = SpinType.DualRing, FixedCssClass = _fixedDualRingCssClass },
            new (){ Type = SpinType.SpinIOS, FixedCssClass = _fixedSpinIosCssClass, InnerDivCount = 12 },
        };

        public static Item GetMetadataItem(SpinType type) =>
            _elements.First(p => p.Type == type);
    }
}
