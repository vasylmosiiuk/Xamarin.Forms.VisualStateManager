using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    [ContentProperty(nameof(Animations))]
    public sealed class Storyboard : BindableObject
    {
        private Duration _duration = Duration.Automatic;

        public Duration Duration
        {
            get
            {
                if (_duration == Duration.Automatic)
                {
                    return new Duration(Animations.Where(x => x.Duration.HasTimeSpan)
                        .Max(x => x.BeginTime + x.Duration.TimeSpan));
                }
                return _duration;
            }
            set => _duration = value;
        }

        public Easing Easing { get; set; } = Easing.Linear;
        public IList<IStoryboardAnimation> Animations { get; } = new List<IStoryboardAnimation>();
    }

    public static class StoryboardExtensions
    {
        public static Animation ToAnimation(this Storyboard storyboard)
        {
            double durationMs;
            if (storyboard.Duration.HasTimeSpan)
                durationMs = storyboard.Duration.TimeSpan.TotalMilliseconds;
            else
                durationMs = storyboard.Animations.Where(x => x.Duration.HasTimeSpan)
                    .Max(x => x.BeginTime + x.Duration.TimeSpan).TotalMilliseconds;

            var xFormsRootAnimation = new Animation((_) => { }, 0.0, durationMs, storyboard.Easing);

            foreach (var animation in storyboard.Animations)
            {
                var animationDuration = animation.Duration;
                var animationDurationMs = 0.0;
                if (animationDuration.HasTimeSpan)
                    animationDurationMs = animationDuration.TimeSpan.TotalMilliseconds;
                else if (animationDuration == Duration.Automatic)
                    animationDurationMs = 0;
                else if (animationDuration == Duration.Forever)
                    animationDurationMs = durationMs;

                var beginAt = Math.Max(0, animation.BeginTime.TotalMilliseconds / durationMs);
                var finishAt = Math.Max(0, (animation.BeginTime.TotalMilliseconds+animationDurationMs) / durationMs);

                var xFormsAnimation = new Animation(Update(animation, beginAt, finishAt, animation.Update), beginAt, finishAt,
                    storyboard.Easing);
                xFormsRootAnimation = xFormsRootAnimation.WithConcurrent(xFormsAnimation, beginAt, finishAt);
            }


            return xFormsRootAnimation;
        }

        private static Action<double> Update(IStoryboardAnimation animation, double beginAt, double finishAt, Action<double> update)
        {
            var duration = finishAt - beginAt;
            return xGlobal =>
            {
                var x = (xGlobal - beginAt) / duration;
                if (double.IsNaN(x)) x = 1.0;
                if (x >= 0.0 && x <= 1.0)
                    update(x);
            };
        }

        public static Duration CalculateExactAnimationDurationMs(this IStoryboardAnimation animation)
        {
            var duration = animation.Duration;
            if (animation.RepeatBehavior)
                return Duration.Forever;

            return duration;
        }
    }
}