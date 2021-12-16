//---------------------------------------------------------------------------------------------------------------------
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

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public SpinType Type { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        private string CssClass { get; set; } = string.Empty;

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
                _ => _fixedSpinCssClass,
            };
        }
    }
}
