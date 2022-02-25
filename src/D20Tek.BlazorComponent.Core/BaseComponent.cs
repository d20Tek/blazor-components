//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public abstract class BaseComponent : ComponentBase
    {
        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public Size Size { get; set; } = Size.Small;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        protected string? CssClass { get; set; } = null;

        protected string? CssStyles { get; set; } = null;

        protected override void OnParametersSet()
        {
            this.CssClass = this.CalculateCssClasses();
            this.CssStyles = this.CalculateCssStyles();
        }

        protected abstract string? CalculateCssClasses();

        protected abstract string? CalculateCssStyles();
    }
}
