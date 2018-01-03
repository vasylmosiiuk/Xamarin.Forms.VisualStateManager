using Xamarin.Forms;

namespace Forms.Media.Animations.Abstractions
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