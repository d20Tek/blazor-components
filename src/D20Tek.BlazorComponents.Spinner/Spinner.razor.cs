//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : ComponentBase
    {
        private const string _fixedCssSpinnerMain = "spinner-area-main";
        private const string _attributNameClass = "class";
        private const string _attributNameStyle = "style";
        private const string _styleNameColor = "color:";
        private const string _styleNameSecondaryColor = "--spinner-secondary-color:";
        private static readonly char[] _charTrimList = { ',', ' ' };

        [Parameter]
        public bool IsVisible { get; set; } = true;

        [Parameter]
        public SpinType Type { get; set; }

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public string Color { get; set; } = string.Empty;

        [Parameter]
        public string SecondaryColor { get; set; } = string.Empty;

        [Parameter]
        public Placement LabelPlacement { get; set; } = Placement.Bottom;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> RemainingAttributes { get; set; } = new Dictionary<string, object>();

        private string CssClass { get; set; } = string.Empty;

        private string? CssStyles { get; set; } = null;

        private string? LabelCssClass { get; set; } = null;

        private bool HasLabel => !string.IsNullOrWhiteSpace(this.Label);

        private SpinTypeMetadata.Item TypeMetadata { get; set; } = SpinTypeMetadata.GetMetadataItem(SpinType.Ring);

        protected override void OnParametersSet()
        {
            this.TypeMetadata = SpinTypeMetadata.GetMetadataItem(this.Type);
            this.CssClass = this.CalculateCssClasses();
            this.CssStyles = this.CalculateCssStyles();
        }

        private string CalculateCssClasses()
        {
            var result = this.TypeMetadata.FixedCssClass;

            if (this.HasLabel)
            {
                result += $" {_fixedCssSpinnerMain}";
                this.LabelCssClass = LabelPlacementMetadata.GetPlacementCss(this.LabelPlacement);
            }

            this.RemainingAttributes.TryGetValue(_attributNameClass, out var value);
            if (value != null)
            {
                result += $" {value}";
            }

            return result;
        }

        private string? CalculateCssStyles()
        {
            string tempStyles = string.Empty;
            this.RemainingAttributes.TryGetValue(_attributNameStyle, out var style);
            if (style != null)
            {
                tempStyles = $"{style}; ";
            }

            if (string.IsNullOrWhiteSpace(this.Color) == false)
            {
                tempStyles += $"{_styleNameColor} {this.Color}; ";
            }

            if (string.IsNullOrWhiteSpace(this.SecondaryColor) == false)
            {
                tempStyles += $"{_styleNameSecondaryColor} {this.SecondaryColor}; ";
            }

            tempStyles = tempStyles.Trim(_charTrimList);
            return (string.IsNullOrEmpty(tempStyles) ? null : tempStyles);
        }
    }
}
