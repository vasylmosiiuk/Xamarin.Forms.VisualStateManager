using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public sealed class DoubleAnimation : StoryboardAnimationBase
    {
        public double? From { get; set; }
        public double To { get; set; }

        public override object Prepare(VisualElement target) => new State(target, (double)target.GetValue(TargetProperty), From ?? (double) target.GetValue(TargetProperty));

        public override void Update(double x, object stateRaw)
        {
            if (stateRaw is State state)
            {
                var value = state.From + (To - state.From) * x;
                state.Target.SetValue(TargetProperty, value);
            }
        }

        public override void Cleanup(object stateRaw)
        {
            if (stateRaw is State state)
            {
                if (FillBehavior == FillBehavior.Stop)
                {
                    state.Target.SetValue(TargetProperty, state.Default);
                }
            }
        }

        private class State
        {
            public readonly double Default;
            public readonly double From;
            public readonly VisualElement Target;

            public State(VisualElement target, double @default, double from)
            {
                Target = target;
                From = from;
                Default = @default;
            }
        }
    }

    public sealed class FloatAnimation : StoryboardAnimationBase
    {
        public float? From { get; set; }
        public float To { get; set; }


        public override object Prepare(VisualElement target)
        {
            return new State(target, From ?? (float) target.GetValue(TargetProperty));
        }

        public override void Update(double x, object stateRaw)
        {
            if (stateRaw is State state)
            {
                var value = (float) (state.From + (To - state.From) * x);
                state.Target.SetValue(TargetProperty, value);
            }
        }

        public override void Cleanup(object stateRaw)
        {
            if (stateRaw is State state)
            {
                if (FillBehavior == FillBehavior.Stop)
                {
                    state.Target.SetValue(TargetProperty, state.From);
                }
            }
        }

        private class State
        {
            public readonly float From;
            public readonly VisualElement Target;

            public State(VisualElement target, float from)
            {
                Target = target;
                From = from;
            }
        }
    }
}