using System;
using Forms.VisualStateManager.Abstractions;

namespace Forms.VisualStateManager.Animations
{
    public sealed class SingleAnimation : LinearPrimitiveAnimation<float>
    {
        protected override void Update(double x, LinearAnimationState<float> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (float)value);
        }
    }
}