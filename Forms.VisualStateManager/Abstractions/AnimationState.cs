using Xamarin.Forms;

namespace Forms.VisualStateManager.Abstractions
{
    public abstract class AnimationState
    {
        public readonly BindableObject Target;

        protected AnimationState(BindableObject target)
        {
            Target = target;
        }
    }
}