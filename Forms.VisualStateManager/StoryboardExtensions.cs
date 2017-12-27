﻿using System;
using System.Linq;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public static class StoryboardExtensions
    {
        public static Animation ToAnimation(this Storyboard storyboard, VisualElement target)
        {
            double durationMs;
            if (storyboard.Duration.HasTimeSpan)
                durationMs = storyboard.Duration.TimeSpan.TotalMilliseconds;
            else
                durationMs = storyboard.Animations.Where(x => x.Duration.HasTimeSpan)
                    .Max(x => x.BeginTime + x.Duration.TimeSpan).TotalMilliseconds;

            var xFormsRootAnimation = new Animation(_ => { }, 0.0, durationMs, storyboard.Easing);

            foreach (var animation in storyboard.Animations)
            {
                var animationDuration = animation.CalculateExactAnimationDuration();
                var animationDurationMs = 0.0;
                if (animationDuration.HasTimeSpan)
                    animationDurationMs = animationDuration.TimeSpan.TotalMilliseconds;
                else if (animationDuration == Duration.Automatic)
                    animationDurationMs = 0;
                else if (animationDuration == Duration.Forever)
                    animationDurationMs = durationMs;

                var beginAt = Math.Max(0, animation.BeginTime.TotalMilliseconds / durationMs);
                var finishAt = Math.Max(0, (animation.BeginTime.TotalMilliseconds + animationDurationMs) / durationMs);

                Animation xFormsAnimation;

                var state = animation.Prepare(target);
                if (animation.RepeatBehavior.RepeatEnabled)
                    xFormsAnimation = new Animation(RepeatUpdate(state,
                            animation.Duration.ToTimeSpan().TotalMilliseconds / durationMs, beginAt,
                            finishAt, animation.AutoReverse, animation.Update, animation.Easing), beginAt, finishAt,
                        storyboard.Easing, ()=> animation.Cleanup(state));
                else
                    xFormsAnimation = new Animation(Update(state, beginAt, finishAt, animation.Update, animation.Easing),
                        beginAt, finishAt, storyboard.Easing, () => animation.Cleanup(state));
                xFormsRootAnimation = xFormsRootAnimation.WithConcurrent(xFormsAnimation, beginAt, finishAt);
            }


            return xFormsRootAnimation;
        }

        private static Action<double> RepeatUpdate(object state, double singleAnimationDuration, double beginAt, double finishAt,
            bool autoReverse, Action<double, object> update, Easing easing)
        {
            return xGlobal =>
            {
                if (xGlobal < finishAt)
                {
                    var timesElapsed = (uint) ((xGlobal - beginAt) / singleAnimationDuration);
                    var x = (xGlobal - beginAt - timesElapsed * singleAnimationDuration) / singleAnimationDuration;
                    if (double.IsNaN(x)) x = 1.0;

                    if (easing != null)
                        x = easing.Ease(x);
                    if (autoReverse && timesElapsed % 2 == 1)
                        x = 0.999999 - x;
                    if (x >= 0.0 && x <= 1.0)
                        update(x, state);
                }
            };
        }

        private static Action<double> Update(object state, double beginAt, double finishAt, Action<double, object> update, Easing easing)
        {
            var duration = finishAt - beginAt;
            return xGlobal =>
            {
                var x = (xGlobal - beginAt) / duration;
                if (double.IsNaN(x)) x = 1.0;

                if (easing != null)
                    x = easing.Ease(x);
                if (x >= 0.0 && x <= 1.0)
                    update(x, state);
            };
        }

        public static Duration CalculateExactAnimationDuration(this IStoryboardAnimation animation)
        {
            var singleAnimationDuration = animation.Duration;
            var repeatBehavior = animation.RepeatBehavior;
            if (repeatBehavior == RepeatBehavior.Forever)
                return Duration.Forever;
            if (singleAnimationDuration.HasTimeSpan)
            {
                if (repeatBehavior.HasCount)
                    return new Duration(singleAnimationDuration.TimeSpan.MulSafety(repeatBehavior.Count));
                if (repeatBehavior.HasDuration)
                    return new Duration(repeatBehavior.Duration);
            }


            return singleAnimationDuration;
        }

        public static TimeSpan ToTimeSpan(this Duration duration)
        {
            if (duration.HasTimeSpan)
                return duration.TimeSpan;
            if (duration == Duration.Automatic)
                return TimeSpan.Zero;
            if (duration == Duration.Forever)
                return TimeSpan.FromDays(2);

            throw new InvalidOperationException("Unknown duration type");
        }

        //https://stackoverflow.com/a/45205696
        public static TimeSpan SumSafety(this TimeSpan ts1, TimeSpan ts2)
        {
            bool sign1 = ts1 < TimeSpan.Zero, sign2 = ts2 < TimeSpan.Zero;

            if (sign1 && sign2)
            {
                if (TimeSpan.MinValue - ts1 > ts2)
                    return TimeSpan.MinValue;
            }
            else if (!sign1 && !sign2)
            {
                if (TimeSpan.MaxValue - ts1 < ts2)
                    return TimeSpan.MaxValue;
            }

            return ts1 + ts2;
        }

        public static TimeSpan MulSafety(this TimeSpan ts1, uint times)
        {
            var sign1 = ts1 < TimeSpan.Zero;

            if (sign1)
                return TimeSpan.Zero;
            if (TimeSpan.MaxValue.TotalMilliseconds - ts1.TotalMilliseconds * times < 0)
                return TimeSpan.MaxValue;

            return TimeSpan.FromMilliseconds(ts1.TotalMilliseconds * times);
        }
    }
}