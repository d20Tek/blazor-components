//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class ContentSpinner : BaseComponent
    {
        public ContentSpinner()
        {
            this.Size = Size.None;
        }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        private bool IsSizeRequired => this.Size != Size.None;

        protected override string? CalculateCssClasses()
        {
            var result = new CssBuilder(SpinnerConstants.ContentCssSpinnerMain)
                             .AddClassFromAttributes(this.RemainingAttributes)
                             .Build();
            return result;
        }

        protected override string? CalculateCssStyles()
        {
            var result = new StyleBuilder()
                .AddStyleFromAttributes(this.RemainingAttributes)
                .AddStyle(SpinnerConstants.StyleNameWidth, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                .AddStyle(SpinnerConstants.StyleNameHeight, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                .Build();

            return result;
        }
    }
}
