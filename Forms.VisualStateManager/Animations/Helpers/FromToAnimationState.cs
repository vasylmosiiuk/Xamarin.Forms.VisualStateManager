using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class AnimationState
    {
        public readonly VisualElement Target;

        protected AnimationState(VisualElement target)
        {
            Target = target;
        }
    }

    public class FromToAnimationState<T> : AnimationState
    {
        public readonly T From;
        public readonly T StoredValue;

        public FromToAnimationState(VisualElement target, T storedValue, T @from) : base(target)
        {
            StoredValue = storedValue;
            From = @from;
        }
    }
}