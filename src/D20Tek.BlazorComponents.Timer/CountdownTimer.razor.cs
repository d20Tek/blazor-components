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

        public string TimerDisplay { get; private set; } = string.Empty;

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

            return (int)delta.TotalSeconds;
        }
    }
}
