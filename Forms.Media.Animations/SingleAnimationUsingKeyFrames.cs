﻿using Forms.Media.Animations.Abstractions;

namespace Forms.Media.Animations
{
    public class SingleAnimationUsingKeyFrames : KeyFrameAnimation<float>
    {
    }

    public class DiscreteSingleKeyFrame : DiscreteKeyFrame<float>
    {
    }

    public class LinearSingleKeyFrame : LinearKeyFrame<float>
    {
        protected override float GetInterpolatedValue(double x, float initialValue)
        {
            return (float) (initialValue + (Value - initialValue) * x);
        }
    }
}
