using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public sealed class Int16Animation : FromToPrimitiveAnimation<FromToAnimationState<short>, short>
    {
        protected override void Update(double x, FromToAnimationState<short> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (short)value);
        }
    }
}