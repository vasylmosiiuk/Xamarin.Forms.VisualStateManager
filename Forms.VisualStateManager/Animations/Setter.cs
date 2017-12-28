using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Internals;

namespace Forms.VisualStateManager.Animations
{
    public class Setter : IStoryboardAnimation
    {
        private readonly Duration _duration = new Duration(TimeSpan.Zero);
        private readonly RepeatBehavior _repeatBehavior = new RepeatBehavior();
        private object _convertedValue;
        private BindableProperty _targetProperty;
        private object _value;
        private bool _valueSet;

        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueSet = true;
                RefreshValue();
            }
        }

        public BindableProperty TargetProperty
        {
            get => _targetProperty;
            set
            {
                _targetProperty = value;
                RefreshValue();
            }
        }

        public FillBehavior FillBehavior { get; set; } = FillBehavior.HoldEnd;
        public Easing Easing { get; set; } = Easing.Linear;
        public TimeSpan BeginTime { get; set; }

        public Duration Duration
        {
            get => _duration;
            set => throw new InvalidOperationException($"nameof(Duration) can't be changed");
        }

        public RepeatBehavior RepeatBehavior
        {
            get => _repeatBehavior;
            set => throw new InvalidOperationException($"nameof(AutoReverse) isn't supported");
        }

        public bool AutoReverse
        {
            get => false;
            set => throw new InvalidOperationException($"nameof(AutoReverse) isn't supported");
        }

        public object StoreVisualState(VisualElement target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            return new State(target, target.GetValue(TargetProperty));
        }

        public void Update(double x, object stateRaw)
        {
            if (stateRaw is State state)
            {
                if (Math.Abs(x - 1.0) < double.Epsilon)
                {
                    state.Target.SetValue(TargetProperty, _convertedValue);
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw?.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        public void RestoreVisualState(VisualElement target, object stateRaw)
        {
            if (stateRaw == null || target == null) return;
            if (stateRaw is State state)
            {
                if (state.Target == target)
                {
                    if (FillBehavior == FillBehavior.Stop)
                        target.SetValue(TargetProperty, state.StoredValue);
                }
                else
                    throw new InvalidOperationException(
                        $"Target ({target.GetType()}), specified for restore isn't same with original target ({state.Target.GetType()})");
            }
            else
            {
                throw new InvalidOperationException(
                    $"State ({stateRaw.GetType()}), specified for restore isn't compatible with this animation");
            }
        }

        private void RefreshValue()
        {
            if (_valueSet)
            {
                Func<MemberInfo> mInfoRetriever = null;
                    _convertedValue = _value?.ConvertTo(TargetProperty.ReturnType, mInfoRetriever,
                        new XamlServiceProvider());
            }
        }

        private class State
        {
            public readonly object StoredValue;
            public readonly VisualElement Target;

            public State(VisualElement target, object storedValue)
            {
                Target = target;
                StoredValue = storedValue;
            }
        }
    }
}