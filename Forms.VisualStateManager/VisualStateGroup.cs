using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    [ContentProperty(nameof(States))]
    public class VisualStateGroup : BindableObject
    {
        private static readonly BindablePropertyKey TransitionsPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Transitions), typeof(VisualTransitionCollection),
                typeof(VisualStateGroup), null, defaultValueCreator: (_) => new VisualTransitionCollection());

        public static readonly BindableProperty TransitionsProperty = TransitionsPropertyKey.BindableProperty;

        private static readonly BindablePropertyKey StatesPropertyKey =
            BindableProperty.CreateReadOnly(nameof(States), typeof(VisualStateCollection), typeof(VisualStateGroup),
                null, defaultValueCreator: (_) => new VisualStateCollection());

        public static readonly BindableProperty StatesProperty = StatesPropertyKey.BindableProperty;

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(VisualStateGroup), default(string));

        public static readonly BindablePropertyKey CurrentStatePropertyKey = BindableProperty.CreateReadOnly(
            nameof(CurrentState),
            typeof(VisualState), typeof(VisualStateGroup), default(VisualState));

        public static readonly BindableProperty CurrentStateProperty = CurrentStatePropertyKey.BindableProperty;
        private readonly List<VisualTransition> _generatedTransitions = new List<VisualTransition>();

        public VisualStateCollection States
        {
            get => (VisualStateCollection) GetValue(StatesProperty);
            private set => SetValue(StatesPropertyKey, value);
        }

        public VisualTransitionCollection Transitions
        {
            get => (VisualTransitionCollection) GetValue(TransitionsProperty);
            private set => SetValue(TransitionsPropertyKey, value);
        }

        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public VisualState CurrentState
        {
            get => (VisualState) GetValue(CurrentStateProperty);
            private set => SetValue(CurrentStatePropertyKey, value);
        }

        public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanging;
        public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanged;

        internal void GoToState(VisualElement element, string stateName, bool useTransitions)
        {
            var currentState = CurrentState;
            var currentStateName = currentState?.Name;

            if (currentStateName == stateName) return;

            var stateToSet = States.FirstOrDefault(x => x.Name == stateName);
            if (stateToSet == null) return;

            var handler = element.GetValue(VisualStateManager.AnimationHandlerProperty);
            var animationGroupHandler = $"{handler}_{Name}";

            element.AbortAnimation(animationGroupHandler);

            RaiseCurrentStateChanging(currentState, stateToSet, element);

            if (useTransitions)
            {
                var transition = ResolveVisualTransition(currentStateName, stateName);
                transition = transition ?? GenerateVisualTransition(element, currentStateName, stateName);
                //var tran
                //var animation = new Animation().Add();
                //var animation = transition.Storyboard.ToAnimation();
                //var duration = (uint) 0;
                //element.Animate(animationGroupHandler, animation, length: duration, easing: );
            }
            else
            {
                var duration = stateToSet.Storyboard.Duration;
                var animationLength = (uint)duration.ToTimeSpan().TotalMilliseconds;

                var animation = stateToSet.Storyboard.ToAnimation();
                element.Animate(animationGroupHandler, animation, length: animationLength, finished: (x, cancelled) => {});
            }
            
            CurrentState = stateToSet;
            RaiseCurrentStateChanged(currentState, stateToSet, element);
        }

        private void RaiseCurrentStateChanging(VisualState fromState, VisualState toState, VisualElement element)
        {
            CurrentStateChanging?.Invoke(this, new VisualStateChangedEventArgs(fromState, toState, element));
        }

        private void RaiseCurrentStateChanged(VisualState fromState, VisualState toState, VisualElement element)
        {
            CurrentStateChanged?.Invoke(this, new VisualStateChangedEventArgs(fromState, toState, element));
        }

        protected virtual VisualTransition GenerateVisualTransition(VisualElement element, string fromState,
            string toState)
        {
            var transition = new VisualTransition
            {
                From = fromState,
                To = toState,
                Storyboard = GenerateVisualTransitionStoryboard(element, fromState, toState)
            };

            _generatedTransitions.Add(transition);

            return transition;
        }

        private Storyboard GenerateVisualTransitionStoryboard(VisualElement element, string fromState, string toState)
        {
            return new Storyboard();
        }

        protected virtual VisualTransition ResolveVisualTransition(string fromVisualState, string toVisualState)
        {
            bool ExplicitTransitionsFilter(VisualTransition t) =>
                t.From == fromVisualState || t.To == toVisualState || t.From == null && t.To == null;

            bool GeneratedTransitionsFilter(VisualTransition t) => t.From == fromVisualState && t.To == toVisualState;
            int KeySelector(VisualTransition t) => CalculateTransitionWeight(t, fromVisualState, toVisualState);

            var explicitTransitions = Transitions.Where(ExplicitTransitionsFilter).OrderByDescending(KeySelector);

            var transition = explicitTransitions.FirstOrDefault();
            transition = transition ?? _generatedTransitions.FirstOrDefault(GeneratedTransitionsFilter);

            return transition;
        }

        protected virtual int CalculateTransitionWeight(VisualTransition transition, string fromVisualState,
            string toVisualState)
        {
            if (transition.From == null && transition.To == null)
                return 0; //Explicit default transition

            var weight = 0;
            if (transition.From == fromVisualState) weight += 2; //Equality of 'from' state is more powerful
            if (transition.To == toVisualState) weight += 1;

            //so, possible to retrive 0 - default, 1 - when to states equals, 2 - when from states equals, 3 - full equal
            return weight;
        }
    }

    public class VisualStateCollection : ObservableCollection<VisualState>
    {
    }

    public class VisualTransitionCollection : ObservableCollection<VisualTransition>
    {
    }
}