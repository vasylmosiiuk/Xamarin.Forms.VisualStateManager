using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class FromToAnimation<TState, TStateValue> : StoryboardAnimationBase<TState>
        where TState : FromToAnimationState<TStateValue> where TStateValue : class
    {
        public TStateValue From { get; set; }
        public TStateValue To { get; set; }

        protected override TState StoreVisualState(VisualElement target)
        {
            var currentValue = (TStateValue)target.GetValue(TargetProperty);
            return (TState)new FromToAnimationState<TStateValue>(target, currentValue, From ?? currentValue);
        }

        protected override void RestoreVisualState(VisualElement target, TState state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }

    public abstract class FromToPrimitiveAnimation<TState, TStateValue> : StoryboardAnimationBase<TState>
        where TState : FromToAnimationState<TStateValue> where TStateValue : struct 
    {
        public TStateValue? From { get; set; }
        public TStateValue To { get; set; }

        protected override TState StoreVisualState(VisualElement target)
        {
            var currentValue = (TStateValue)target.GetValue(TargetProperty);
            return (TState)new FromToAnimationState<TStateValue>(target, currentValue, From ?? currentValue);
        }

        protected override void RestoreVisualState(VisualElement target, TState state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }
    }
}