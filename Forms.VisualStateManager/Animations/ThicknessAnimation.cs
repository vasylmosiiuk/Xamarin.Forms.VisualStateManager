using Forms.VisualStateManager.Animations.Helpers;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public sealed class ThicknessAnimation : FromToPrimitiveAnimation<FromToAnimationState<Thickness>, Thickness>
    {
        protected override void Update(double x, FromToAnimationState<Thickness> state)
        {
            var from = state.From;
            var to = To;
            var left = from.Left + (to.Left - from.Left) * x;
            var right = from.Right + (to.Right - from.Right) * x;
            var top = from.Top + (to.Top - from.Top) * x;
            var bottom = from.Bottom + (to.Bottom - from.Bottom) * x;

            var value = new Thickness(left, top, right, bottom);
            state.Target.SetValue(TargetProperty, value);
        }
    }
}