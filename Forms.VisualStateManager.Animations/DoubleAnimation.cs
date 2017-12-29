using Forms.VisualStateManager.Abstractions;

namespace Forms.VisualStateManager.Animations
{
    public sealed class DoubleAnimation : LinearPrimitiveAnimation<double>
    {
        protected override void Update(double x, LinearAnimationState<double> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, value);
        }
    }
}