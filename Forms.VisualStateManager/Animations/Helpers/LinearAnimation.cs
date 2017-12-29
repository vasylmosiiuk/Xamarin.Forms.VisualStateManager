using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class LinearAnimation<TAnimationValue> : AnimationBase<LinearAnimationState<TAnimationValue>>
        where TAnimationValue : class
    {
        public TAnimationValue From { get; set; }
        public TAnimationValue To { get; set; }

        protected override LinearAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new LinearAnimationState<TAnimationValue>(target, currentValue, From ?? currentValue);
        }

        protected override void RestoreAnimationState(BindableObject target,
            LinearAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }

    public abstract class LinearPrimitiveAnimation<TAnimationValue> : AnimationBase<LinearAnimationState<TAnimationValue>>
        where TAnimationValue : struct
    {
        public TAnimationValue? From { get; set; }
        public TAnimationValue To { get; set; }

        protected override LinearAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new LinearAnimationState<TAnimationValue>(target, currentValue, From ?? currentValue);
        }

        protected override void RestoreAnimationState(BindableObject target,
            LinearAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }
}