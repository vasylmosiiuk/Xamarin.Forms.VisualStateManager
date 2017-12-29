﻿namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class DiscreteKeyFrame<TValue> : KeyFrame<TValue>
    {
        public override void Update(double x, KeyFrameAnimationState<TValue> state)
        {
            state.Target.SetValue(state.TargetProperty, Value);
        }
    }
}