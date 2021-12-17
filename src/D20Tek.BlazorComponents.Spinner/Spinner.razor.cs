//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Spinner : ComponentBase
    {
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

        private SpinTypeMetadata.Item TypeMetadata { get; set; } = SpinTypeMetadata.GetMetadataItem(SpinType.Ring);

        protected override void OnParametersSet()
        {
            this.TypeMetadata = SpinTypeMetadata.GetMetadataItem(this.Type);
            this.CssClass = this.TypeMetadata.FixedCssClass;

            this.RemainingAttributes.TryGetValue("class", out var value);
            if (value != null)
            {
                this.CssClass += $" {value}";
            }
        }
    }
}
