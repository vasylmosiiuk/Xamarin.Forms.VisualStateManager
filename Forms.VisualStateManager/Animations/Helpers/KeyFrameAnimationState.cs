using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public class KeyFrameAnimationState<T> : AnimationState
    {
        public readonly BindableProperty TargetProperty;
        public readonly T StoredValue;
        public T PreviousKeyFrameValue;

        public KeyFrameAnimationState(BindableObject target, BindableProperty targetProperty, T storedValue) : base(target)
        {
            TargetProperty = targetProperty;
            StoredValue = storedValue;
            PreviousKeyFrameValue = storedValue;
        }
    }
}