using Forms.VisualStateManager.Animations.Helpers;

namespace Forms.VisualStateManager.Animations
{
    public class DoubleAnimationUsingKeyFrames : KeyFrameAnimation<double>
    {
    }

    public class DiscreteDoubleKeyFrame : DiscreteKeyFrame<double>
    {
    }

    public class LinearDoubleKeyFrame : LinearKeyFrame<double>
    {
        protected override double GetInterpolatedValue(double x, double initialValue)
        {
            return initialValue + (Value - initialValue) * x;
        }
    }
}