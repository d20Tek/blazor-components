//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class SpanTimer : RadialTimer
    {
        private int _timerDuration = 30;
        private TimeSpan _timerDurationSpan = new TimeSpan(0, 0, 30);

        public override int TimerDuration
        {
            get => this._timerDuration;
            set
            {
                _validTimeRange.AssertInRange(value, nameof(TimerDuration));
                this._timerDuration = value;
            }
        }

        [Parameter]
        public TimeSpan TimerDurationSpan
        {
            get => _timerDurationSpan;
            set
            {
                this.TimerDuration = (int)value.TotalSeconds;
                this._timerDurationSpan = value;
            }
        }
    }
}
