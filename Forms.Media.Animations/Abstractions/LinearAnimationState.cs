﻿using Xamarin.Forms;

namespace Forms.Media.Animations.Abstractions
{
    public class LinearAnimationState<T> : AnimationState
    {
        public readonly T From;
        public readonly T StoredValue;

        public LinearAnimationState(BindableObject target, T storedValue, T from) : base(target)
        {
            StoredValue = storedValue;
            From = from;
        }
    }
}