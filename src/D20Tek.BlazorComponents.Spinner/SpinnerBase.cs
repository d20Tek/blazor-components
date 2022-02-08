//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents
{
    public abstract class SpinnerBase : BaseComponent
    {
        [Parameter]
        public Size Size { get; set; } = Size.Small;
    }
}
