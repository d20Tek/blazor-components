//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : BaseComponent
    {
        [Parameter]
        public SpinType Type { get; set; }

        [Parameter]
        public string Color { get; set; } = string.Empty;

        [Parameter]
        public string SecondaryColor { get; set; } = string.Empty;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public Placement LabelPlacement { get; set; } = Placement.Bottom;

        private string? LabelCssClass { get; set; } = null;

        private bool HasLabel => !string.IsNullOrWhiteSpace(this.Label);

        private bool IsSizeRequired => (this.Size != Size.None && this.Size != Size.Small);

        private SpinTypeMetadata.Item TypeMetadata { get; set; } = SpinTypeMetadata.GetMetadataItem(SpinType.Ring);

        protected override string? CalculateCssClasses()
        {
            this.TypeMetadata = SpinTypeMetadata.GetMetadataItem(this.Type);
            var result = new CssBuilder(this.TypeMetadata.FixedCssClass)
                             .AddClass(SpinnerConstants.FixedCssSpinnerMain, HasLabel)
                             .AddClassFromAttributes(this.RemainingAttributes)
                             .Build();

            if (this.HasLabel)
            {
                this.LabelCssClass = LabelPlacementMetadata.GetPlacementCss(this.LabelPlacement);
            }

            return result;
        }

        protected override string? CalculateCssStyles()
        {
            var result = new StyleBuilder()
                .AddStyleFromAttributes(this.RemainingAttributes)
                .AddStyle(SpinnerConstants.StyleNameColor, this.Color, () => {
                    return string.IsNullOrWhiteSpace(this.Color) == false; })
                .AddStyle(SpinnerConstants.StyleNameSecondaryColor, this.SecondaryColor, () => {
                    return string.IsNullOrWhiteSpace(this.SecondaryColor) == false; })
                .AddStyle(SpinnerConstants.StyleNameWidth, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                .AddStyle(SpinnerConstants.StyleNameHeight, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                .Build();

            return result;
        }
    }
}
