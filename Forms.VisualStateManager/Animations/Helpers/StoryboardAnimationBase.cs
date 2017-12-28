using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class StoryboardAnimationBase<TState> : IStoryboardAnimation
        where TState : AnimationState
    {
        public FillBehavior FillBehavior { get; set; }
        public Easing Easing { get; set; }
        public TimeSpan BeginTime { get; set; }
        public Duration Duration { get; set; } = new Duration(TimeSpan.Zero);
        public BindableProperty TargetProperty { get; set; }
        public RepeatBehavior RepeatBehavior { get; set; }
        public bool AutoReverse { get; set; }

        void IStoryboardAnimation.RestoreVisualState(VisualElement target, object stateRaw)
        {
            if (stateRaw == null || target == null) return;
            if (stateRaw is TState state)
            {
                if (state.Target == target)
                    RestoreVisualState(target, state);
                else
                    throw new InvalidOperationException(
                        $"Target ({target.GetType()}), specified for restore isn't same with original target ({state.Target.GetType()})");
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        void IStoryboardAnimation.Update(double x, object stateRaw)
        {
            if (stateRaw == null) Update(x, null);
            if (stateRaw is TState state)
            {
                Update(x, state);
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw?.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        object IStoryboardAnimation.StoreVisualState(VisualElement target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return StoreVisualState(target);
        }

        protected virtual void RestoreVisualState(VisualElement target, TState state)
        {
        }

        protected virtual TState StoreVisualState(VisualElement target)
        {
            return default(TState);
        }

        protected abstract void Update(double x, TState state);
    }
}