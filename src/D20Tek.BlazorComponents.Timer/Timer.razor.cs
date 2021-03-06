//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public partial class Timer : RadialTimer
    {
        private int _timerDuration = 30;

        [Parameter]
        public override int TimerDuration
        {
            get => this._timerDuration;
            set
            {
                _validTimeRange.AssertInRange(value, nameof(TimerDuration));
                this._timerDuration = value;
            }
        }

        public Timer()
        {
        }
    }
}
