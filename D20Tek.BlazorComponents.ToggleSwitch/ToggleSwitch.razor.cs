//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class ToggleSwitch : BaseComponent
    {
        private const string _cssToggleSwitchContainer = "form-check form-switch mt-2";

        public ToggleSwitch()
        {
            Size = Size.None;
        }

        [Parameter]
        public bool Checked { get; set; } = true;

        [Parameter]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public string ToggleColor { get; set; } = string.Empty;

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        private string? ToggleBgColorStyle =>
            string.IsNullOrEmpty(ToggleColor) ? null : $"background-color: {ToggleColor}";

        private async Task OnCheckedChanged(ChangeEventArgs args)
        {
            if (args.Value is not null)
            {
                Checked = (bool)args.Value;
                await CheckedChanged.InvokeAsync(Checked);
            }
        }

        protected override string? CalculateCssClasses()
        {
            var result = new CssBuilder(_cssToggleSwitchContainer)
                             .AddClass(ToggleSizeMetadata.GetSizeCss(Size))
                             .AddClassFromAttributes(RemainingAttributes)
                             .Build();
            return result;
        }

        protected override string? CalculateCssStyles()
        {
            var result = new StyleBuilder()
                .AddStyleFromAttributes(RemainingAttributes)
                .Build();

            return result;
        }
    }
}
