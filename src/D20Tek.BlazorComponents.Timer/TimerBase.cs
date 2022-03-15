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

        private sys.Timer? _timer;

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

            this.InitializeTime();
            this._timer = new sys.Timer(this.OnTimerChanged, null, _millisecondsPerSec, _millisecondsPerSec);
        }

        public void ResetTimer()
        {
            this.InitializeTime();

            InvokeAsync(() => this.StateHasChanged());

            if (this._timer != null)
            {
                this._timer.Change(_millisecondsPerSec, _millisecondsPerSec);
            }
        }

        protected virtual void InitializeTime() { }

        protected abstract int ProcessTimerChange();

        internal void OnTimerChanged(object? state)
        {
            var difference = this.ProcessTimerChange();

            if (difference <= 0)
            {
                if (this._timer != null)
                {
                    this._timer.Change(Timeout.Infinite, Timeout.Infinite);
                }

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
