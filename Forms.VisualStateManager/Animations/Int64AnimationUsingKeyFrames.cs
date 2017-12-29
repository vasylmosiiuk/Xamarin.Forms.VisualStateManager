using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public class Int64AnimationUsingKeyFrames : KeyFrameAnimation<long>
    {
    }

    public class DiscreteInt64KeyFrame : DiscreteKeyFrame<long>
    {
    }

    public class LinearInt64KeyFrame : LinearKeyFrame<long>
    {
        protected override long GetInterpolatedValue(double x, long initialValue)
        {
            return (long)(initialValue + (Value - initialValue) * x);
        }
    }
}