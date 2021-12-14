//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : ComponentBase
    {
        private const string _fixedCssClasses = "spinner";

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        private string CssClass { get; set; } = string.Empty;

        protected override void OnParametersSet()
        {
            this.CssClass = _fixedCssClasses;
            this.RemainingAttributes.TryGetValue("class", out var value);
            if (value != null)
            {
                this.CssClass += $" {value}";
            }
        }
    }
}
