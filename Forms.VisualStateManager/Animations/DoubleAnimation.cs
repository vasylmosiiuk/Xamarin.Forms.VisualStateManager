using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public sealed class DoubleAnimation : StoryboardAnimationBase
    {
        private static readonly BindableProperty FromProperty =
            BindableProperty.CreateAttached(Guid.NewGuid().ToString("N"), typeof(double), typeof(DoubleAnimation),
                default(double));

        public double? From { get; set; }
        public double To { get; set; }

        public override void Update(VisualElement target, double x)
        {
            if (!From.HasValue)
                target.SetValue(FromProperty, target.GetValue(TargetProperty));
            
            var from = From ?? (double) target.GetValue(FromProperty);
            var value = from + (To - from) * x;
            target.SetValue(TargetProperty, value);
        }
    }

    public sealed class FloatAnimation : StoryboardAnimationBase
    {
        private static readonly BindableProperty FromProperty =
            BindableProperty.CreateAttached(Guid.NewGuid().ToString("N"), typeof(float), typeof(FloatAnimation),
                default(float));

        public float? From { get; set; }
        public float To { get; set; }

        public override void Update(VisualElement target, double x)
        {
            if (!From.HasValue)
                target.SetValue(FromProperty, target.GetValue(TargetProperty));

            var from = From ?? (float) target.GetValue(FromProperty);
            var value = (float) (from + (To - from) * x);
            target.SetValue(TargetProperty, value);
        }
    }
}