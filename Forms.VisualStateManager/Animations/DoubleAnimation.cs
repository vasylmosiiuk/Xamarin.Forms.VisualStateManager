using System;

namespace Forms.VisualStateManager.Animations
{
    public sealed class DoubleAnimation : StoryboardAnimationBase
    {
        public double? From { get; set; }
        public double To { get; set; }
        public override void Update(double x)
        {
            if (!From.HasValue)
                From = (double)Target.GetValue(TargetProperty);

            var value = From + (To - From) * x;
            Target.SetValue(TargetProperty, value);
        }
    }

    public sealed class FloatAnimation : StoryboardAnimationBase
    {
        public float? From { get; set; }
        public float To { get; set; }
        public override void Update(double x)
        {
            if (!From.HasValue)
                From = (float)Target.GetValue(TargetProperty);

            var value = (float) (From.Value + (To - From.Value) * x);
            Target.SetValue(TargetProperty, value);
        }
    }
}