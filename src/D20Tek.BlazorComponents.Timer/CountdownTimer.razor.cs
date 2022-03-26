//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class CountdownTimer : TimerBase
    {
        private const string _cssTimerMain = "base-timer";

        [Parameter]
        public DateTimeOffset CountdownTarget { get; set; } = DateTimeOffset.Now.AddDays(1);

        [Parameter]
        public string? LabelText { get; set; } = null;

        public string TimerDisplay { get; private set; } = "...";

        private bool HasLabelText => !string.IsNullOrWhiteSpace(this.LabelText);

        protected override string? CalculateCssClasses()
        {
            var result = new CssBuilder(_cssTimerMain)
                             .AddClassFromAttributes(this.RemainingAttributes)
                             .Build();
            return result;
        }

        protected override string? CalculateCssStyles()
        {
            var result = new StyleBuilder()
                .AddStyleFromAttributes(this.RemainingAttributes)
                .Build();

            return result;
        }

        protected override int ProcessTimerChange()
        {
            var delta = this.CountdownTarget - DateTimeOffset.Now;

            this.TimerDisplay = TimeDisplayFormatter.FormatTimeSpanRemaining(
                delta, this.ExpirationMessage);

            return (int)Math.Floor(delta.TotalSeconds);
        }
    }
}
