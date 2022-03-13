//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using sys = System.Threading;

namespace D20Tek.BlazorComponents
{
    public abstract class TimerBase : BaseComponent, IDisposable
    {
        private const int _millisecondsPerSec = 1000;

        private int _timeCounter = 0;
        private sys.Timer? _timer;

        public abstract int TimerDuration { get; set; }

        public string TimerDurationDisplay
        {
            get
            {
                return TimeDisplayFormatter.FormatTimeRemaining(this.TimeRemaining, this.ExpirationMessage);
            }
        }

        [Parameter]
        public EventCallback TimerExpired { get; set; }

        [Parameter]
        public string ExpirationMessage { get; set; } = "Time's up!";

        public int TimeRemaining { get; protected set; } = 0;

        public bool IsDisposed { get; private set; }

        public TimerBase()
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
