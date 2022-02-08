//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;
using sys = System.Threading;

namespace D20Tek.BlazorComponents
{
    public partial class Timer : BaseComponent
    {
        private const int _secondsPerMin = 60;
        private const int _millisecondsPerSec = 1000;
        private const int _fullDashArray = 283;
        private const string _cssTimerMain = "base-timer";
        private const string _cssTimerSizeMedium = "base-timer-md";

        private int _timeRemaining = 0;
        private int _timeCounter = 0;
        private sys.Timer? _timer;
        private bool disposedValue;

        private string TimerPathColorCss => $"stroke: {this.GetRemainingPathColor(this._timeRemaining)}";

        private string TimerPathDashArray => $"{Math.Ceiling(this.CalculateTimeFraction() * _fullDashArray)} {_fullDashArray}";

        [Parameter]
        public int TimerDuration { get; set; } = 30;

        [Parameter]
        public int WarningThreshold { get; set; } = 15;

        [Parameter]
        public int AlertThreshold { get; set; } = 8;

        [Parameter]
        public string RemainingTimeColor { get; set; } = "green";

        [Parameter]
        public string WarningTimeColor { get; set; } = "orange";

        [Parameter]
        public string AlertTimeColor { get; set; } = "red";

        [Parameter]
        public EventCallback TimerExpired { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ResetTimer();

            this._timeRemaining = TimerDuration;
            this._timer = new sys.Timer(this.OnTimerChanged, null, _millisecondsPerSec, _millisecondsPerSec);
        }


        protected override string? CalculateCssClasses()
        {
            var result = new CssBuilder(_cssTimerMain)
                             .AddClass(_cssTimerSizeMedium)
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

        public void ResetTimer()
        {
            this._timeCounter = 0;
            this._timeRemaining = TimerDuration;
            this.StateHasChanged();

            this._timer?.Change(_millisecondsPerSec, _millisecondsPerSec);
        }

        private string FormatTimeRemaining(int time)
        {
            if (time <= 0)
            {
                return "Time's up!";
            }

            var minutes = Math.Floor((decimal)time / _secondsPerMin);
            var seconds = time % _secondsPerMin;

            // The output in MM:SS format
            return $"{minutes}:{seconds:D2}";
        }

        private void OnTimerChanged(object? state)
        {
            this._timeCounter++;
            this._timeRemaining = this.TimerDuration - this._timeCounter;

            if (this._timeRemaining <= 0)

            {
                this._timer?.Change(Timeout.Infinite, Timeout.Infinite);

                this._timeCounter = this.TimerDuration;
                this._timeRemaining = 0;

                this.TimerExpired.InvokeAsync();
            }

            this.StateHasChanged();
        }

        private double CalculateTimeFraction()
        {
            var rawFraction = this._timeRemaining / (double)this.TimerDuration;
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._timer?.Dispose();
                    this._timer = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
