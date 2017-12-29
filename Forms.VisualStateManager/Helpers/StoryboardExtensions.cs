﻿using System;
using System.Linq;
using Forms.VisualStateManager.Abstractions;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Helpers
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

            var xFormsRootAnimation = new Animation(_ => { }, 0.0, durationMs, Easing.Linear);

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

                var state = animation.StoreAnimationState(target);
                if (animation.RepeatBehavior.RepeatEnabled)
                    xFormsAnimation = new Animation(RepeatUpdate(state,
                            animation.Duration.ToTimeSpan().TotalMilliseconds / durationMs, beginAt,
                            finishAt, animation.AutoReverse, animation.Update, animation.Easing), beginAt, finishAt,
                        Easing.Linear, () => animation.RestoreAnimationState(target, state));
                else
                    xFormsAnimation = new Animation(
                        Update(state, beginAt, finishAt, animation.Update, animation.Easing),
                        beginAt, finishAt, Easing.Linear, () => animation.RestoreAnimationState(target, state));
                xFormsRootAnimation = xFormsRootAnimation.WithConcurrent(xFormsAnimation, beginAt, finishAt);
            }


            return xFormsRootAnimation;
        }

        private static Action<double> RepeatUpdate(object state, double singleAnimationDuration, double beginAt,
            double finishAt,
            bool autoReverse, Action<double, object> update, Easing easing)
        {
            if (singleAnimationDuration < double.Epsilon)
                throw new ArgumentOutOfRangeException(nameof(singleAnimationDuration),
                    "Forever animation should have non-zero duration");

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

        private static Action<double> Update(object state, double beginAt, double finishAt,
            Action<double, object> update, Easing easing)
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
    }
}