//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public abstract class RadialTimer : TimerBase
    {
        private const int _fullDashArray = 283;
        private const string _cssTimerMain = "base-timer";
        protected static ValueRange _validTimeRange = new ValueRange(0, 1000000);

        private int _timeCounter = 0;
        private int _warningThreshold = 15;
        private int _alertThreshold = 8;

        protected string TimerElapsedColorCss => $"stroke: {this.ElapsedTimeColor}";

        protected string TimerPathColorCss => $"stroke: {this.GetRemainingPathColor(this.TimeRemaining)}";

        protected string TimerPathDashArray => $"{Math.Ceiling(this.CalculateTimeFraction() * _fullDashArray)} {_fullDashArray}";

        public abstract int TimerDuration { get; set; }

        public string TimerDurationDisplay
        {
            get
            {
                return TimeDisplayFormatter.FormatTimeRemaining(this.TimeRemaining, this.ExpirationMessage);
            }
        }

        [Parameter]
        public int WarningThreshold
        {
            get => _warningThreshold;
            set
            {
                _validTimeRange.AssertInRange(value, nameof(WarningThreshold));
                this._warningThreshold = value;
            }
        }

        [Parameter]
        public int AlertThreshold
        {
            get => this._alertThreshold;
            set
            {
                _validTimeRange.AssertInRange(value, nameof(AlertThreshold));
                this._alertThreshold = value;
            }
        }

        [Parameter]
        public string RemainingTimeColor { get; set; } = "green";

        [Parameter]
        public string WarningTimeColor { get; set; } = "orange";

        [Parameter]
        public string AlertTimeColor { get; set; } = "red";

        [Parameter]
        public string ElapsedTimeColor { get; set; } = "gray";

        public RadialTimer()
        {
        }

        protected override void OnInitialized()
        {
            this.TimeRemaining = TimerDuration;
            base.OnInitialized();
        }

        protected override string? CalculateCssClasses()
        {
            var result = new CssBuilder(_cssTimerMain)
                             .AddClass(TimerSizeMetadata.GetSizeCss(this.Size))
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

        protected override void InitializeTime()
        {
            this._timeCounter = 0;
            this.TimeRemaining = TimerDuration;
        }

        protected override int ProcessTimerChange()
        {
            this._timeCounter++;
            this.TimeRemaining = this.TimerDuration - this._timeCounter;

            if (this.TimeRemaining <= 0)

            {
                this._timeCounter = this.TimerDuration;
                this.TimeRemaining = 0;
            }

            return this.TimeRemaining;
        }

        private double CalculateTimeFraction()
        {
            var rawFraction = this.TimeRemaining / (double)this.TimerDuration;
            return rawFraction - (1 / (double)this.TimerDuration) * (1 - rawFraction);
        }

        private string GetRemainingPathColor(int timeLeft)
        {
            var result = this.RemainingTimeColor;
            if (timeLeft <= this.AlertThreshold)
            {
                result = this.AlertTimeColor;
            }
            else if (timeLeft <= this.WarningThreshold)
            {
                result = this.WarningTimeColor;
            }

            return result;
        }
    }
}
