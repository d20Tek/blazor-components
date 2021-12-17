﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : ComponentBase
    {
        private const string _fixedSpinCssClass = "spinner";
        private const string _fixedPulseCssClass = "spinner-pulse";
        private const string _fixedSquareCssClass = "spinner-square";
        private const string _fixedHourGlassCssClass = "spinner-hourglass";
        private const string _fixedDualRingCssClass = "spinner-dualring";
        private const string _fixedSpinIosCssClass = "spinner-ios";

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public SpinType Type { get; set; }

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        private string CssClass { get; set; } = string.Empty;

        private bool HasLabel => !string.IsNullOrWhiteSpace(this.Label);

        private int InnerDivCount =>
            this.Type switch
            {
                //SpinType.Pulse => _fixedPulseCssClass,
                //SpinType.Square => _fixedSquareCssClass,
                //SpinType.Hourglass => _fixedHourGlassCssClass,
                SpinType.SpinIOS => 12,
                _ => 0,
            };

    protected override void OnParametersSet()
        {
            this.CssClass = this.SpinTypeToCssClass(this.Type);
            this.RemainingAttributes.TryGetValue("class", out var value);
            if (value != null)
            {
                this.CssClass += $" {value}";
            }
        }

        private string SpinTypeToCssClass(SpinType type)
        {
            return type switch
            {
                SpinType.Pulse => _fixedPulseCssClass,
                SpinType.Square => _fixedSquareCssClass,
                SpinType.Hourglass => _fixedHourGlassCssClass,
                SpinType.DualRing =>_fixedDualRingCssClass,
                SpinType.SpinIOS => _fixedSpinIosCssClass,
                _ => _fixedSpinCssClass,
            };
        }
    }
}
