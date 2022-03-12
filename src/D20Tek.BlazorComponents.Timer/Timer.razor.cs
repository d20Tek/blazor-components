﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.AspNetCore.Components;
using sys = System.Threading;

namespace D20Tek.BlazorComponents
{
    public partial class Timer : BaseComponent, IDisposable
    {
        private const int _secondsPerMin = 60;
        private const int _secondsPerHour = _secondsPerMin * 60;
        private const int _millisecondsPerSec = 1000;
        private const int _fullDashArray = 283;
        private const string _cssTimerMain = "base-timer";
        private static ValueRange _validTimeRange = new ValueRange(0, 1000000);

        private int _timeCounter = 0;
        private sys.Timer? _timer;
        private int _timerDuration = 30;
        private int _warningThreshold = 15;
        private int _alertThreshold = 8;

        protected string TimerElapsedColorCss => $"stroke: {this.ElapsedTimeColor}";

        protected string TimerPathColorCss => $"stroke: {this.GetRemainingPathColor(this.TimeRemaining)}";

        protected string TimerPathDashArray => $"{Math.Ceiling(this.CalculateTimeFraction() * _fullDashArray)} {_fullDashArray}";

        [Parameter]
        public int TimerDuration
        {
            get => this._timerDuration;
            set
            {
                _validTimeRange.AssertInRange(value, nameof(TimerDuration));
                this._timerDuration = value;
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

        [Parameter]
        public EventCallback TimerExpired { get; set; }

        [Parameter]
        public string ExpirationMessage { get; set; } = "Time's up!";

        public int TimeRemaining { get; private set; } = 0;

        public bool IsDisposed { get; private set; }

        public Timer()
        {
            this.Size = Size.Medium;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ResetTimer();

            this.TimeRemaining = TimerDuration;
            this._timer = new sys.Timer(this.OnTimerChanged, null, _millisecondsPerSec, _millisecondsPerSec);
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

        public void ResetTimer()
        {
            this._timeCounter = 0;
            this.TimeRemaining = TimerDuration;
            InvokeAsync(() => this.StateHasChanged());

            if (this._timer != null)
            {
                this._timer.Change(_millisecondsPerSec, _millisecondsPerSec);
            }
        }

        protected string FormatTimeRemaining(int time)
        {
            if (time <= 0)
            {
                return this.ExpirationMessage;
            }

            int hours = (int)Math.Floor((decimal)time / _secondsPerHour);
            int remainingTime = time % _secondsPerHour;
            int minutes = (int)Math.Floor((decimal)remainingTime / _secondsPerMin);
            int seconds = remainingTime % _secondsPerMin;

            return FormatTimeRemaining(hours, minutes, seconds);
        }

        internal static string FormatTimeRemaining(int hours, int minutes, int seconds)
        {
            if (hours > 0)
            {
                // The output in HH:MM:SS format
                return $"{hours}:{minutes:D2}:{seconds:D2}";
            }

            // The output in MM:SS format
            return $"{minutes}:{seconds:D2}";
        }

        internal void OnTimerChanged(object? state)
        {
            this._timeCounter++;
            this.TimeRemaining = this.TimerDuration - this._timeCounter;

            if (this.TimeRemaining <= 0)

            {
                if (this._timer != null)
                {
                    this._timer.Change(Timeout.Infinite, Timeout.Infinite);
                }

                this._timeCounter = this.TimerDuration;
                this.TimeRemaining = 0;

                this.TimerExpired.InvokeAsync();
            }

            InvokeAsync(() => this.StateHasChanged());
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

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this._timer?.Dispose();
                    this._timer = null;
                }

                this.IsDisposed = true;
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
