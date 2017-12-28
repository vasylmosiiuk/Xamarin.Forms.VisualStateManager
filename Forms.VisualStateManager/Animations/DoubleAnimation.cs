using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public sealed class DoubleAnimation : FromToPrimitiveAnimation<FromToAnimationState<double>, double>
    {
        protected override void Update(double x, FromToAnimationState<double> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, value);
        }
    }
}