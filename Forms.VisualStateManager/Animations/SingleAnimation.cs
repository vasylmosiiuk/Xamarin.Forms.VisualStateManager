using System;
using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public sealed class SingleAnimation : FromToPrimitiveAnimation<FromToAnimationState<float>, float>
    {
        protected override void Update(double x, FromToAnimationState<float> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (float)value);
        }
    }
}