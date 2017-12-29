﻿using System;
using Forms.VisualStateManager.Helpers;

namespace Forms.VisualStateManager.Animations.Helpers
{
    public abstract class KeyFrame<TValue> : IApplicable
    {
        private bool _isApplied;
        private TimeSpan _keyTime;

        private TValue _value;

        public TimeSpan KeyTime
        {
            get => _keyTime;
            set
            {
                this.ThrowIfApplied();
                _keyTime = value;
            }
        }

        public TValue Value
        {
            get => _value;
            set
            {
                this.ThrowIfApplied();
                _value = value;
            }
        }

        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        public void Apply() => IsApplied = true;

        public abstract void Update(double x, KeyFrameAnimationState<TValue> state);
    }
}