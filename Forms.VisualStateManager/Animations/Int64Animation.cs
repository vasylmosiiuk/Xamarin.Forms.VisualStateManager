﻿using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public sealed class Int64Animation : FromToPrimitiveAnimation<FromToAnimationState<long>, long>
    {
        protected override void Update(double x, FromToAnimationState<long> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (long)value);
        }
    }
}