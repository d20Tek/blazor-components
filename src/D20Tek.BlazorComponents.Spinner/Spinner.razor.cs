//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : ComponentBase
    {
        private const string _fixedCssSpinnerMain = "spinner-area-main";
        private const string _styleNameColor = "color";
        private const string _styleNameSecondaryColor = "--spinner-secondary-color";
        private const string _styleNameWidth = "--spinner-width";
        private const string _styleNameHeight = "--spinner-height";

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public SpinType Type { get; set; }

        [Parameter]
        public string Color { get; set; } = string.Empty;

        [Parameter]
        public string SecondaryColor { get; set; } = string.Empty;

        [Parameter]
        public Size Size { get; set; } = Size.Small;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public Placement LabelPlacement { get; set; } = Placement.Bottom;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        private string? CssClass { get; set; } = null;

        private string? CssStyles { get; set; } = null;

        private string? LabelCssClass { get; set; } = null;

        private bool HasLabel => !string.IsNullOrWhiteSpace(this.Label);

        private bool IsSizeRequired => (this.Size != Size.None && this.Size != Size.Small);

        private SpinTypeMetadata.Item TypeMetadata { get; set; } = SpinTypeMetadata.GetMetadataItem(SpinType.Ring);

        protected override void OnParametersSet()
        {
            this.TypeMetadata = SpinTypeMetadata.GetMetadataItem(this.Type);
            this.CssClass = this.CalculateCssClasses();
            this.CssStyles = this.CalculateCssStyles();
        }

        private string? CalculateCssClasses()
        {
            var result = new CssBuilder(this.TypeMetadata.FixedCssClass)
                             .AddClass(_fixedCssSpinnerMain, HasLabel)
                             .AddClassFromAttributes(this.RemainingAttributes)
                             .Build();

            if (this.HasLabel)
            {
                this.LabelCssClass = LabelPlacementMetadata.GetPlacementCss(this.LabelPlacement);
            }

            return result;
        }

        private string? CalculateCssStyles()
        {
            var result = new StyleBuilder()
                             .AddStyleFromAttributes(this.RemainingAttributes)
                             .AddStyle(_styleNameColor, this.Color, () => {
                                    return string.IsNullOrWhiteSpace(this.Color) == false; })
                             .AddStyle(_styleNameSecondaryColor, this.SecondaryColor, () => {
                                    return string.IsNullOrWhiteSpace(this.SecondaryColor) == false; })
                             .AddStyle(_styleNameWidth, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                             .AddStyle(_styleNameHeight, SpinnerSizeMetadata.GetSizeCss(this.Size), this.IsSizeRequired)
                             .Build();

            return result;
        }
    }
}
