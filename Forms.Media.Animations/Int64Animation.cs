using Forms.Media.Animations.Abstractions;

namespace Forms.Media.Animations
{
    public sealed class Int64Animation : LinearPrimitiveAnimation<long>
    {
        protected override void Update(double x, LinearAnimationState<long> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, (long)value);
        }
    }
}