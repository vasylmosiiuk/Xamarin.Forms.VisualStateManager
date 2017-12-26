using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public class VisualStateManager : BindableObject
    {
        private static readonly BindablePropertyKey AnimationHandlerPropertyKey = BindableProperty.CreateReadOnly(
            "AnimationHandler",
            typeof(string), typeof(VisualStateManager), null,
            defaultValueCreator: GenerateAnimationHandler);

        private static object GenerateAnimationHandler(BindableObject bindable)
        {
            if (bindable is Element element)
                return element.Id.ToString("N");
            return Guid.NewGuid().ToString("N");
        }

        internal static readonly BindableProperty AnimationHandlerProperty =
            AnimationHandlerPropertyKey.BindableProperty;


        public static readonly BindableProperty VisualStateGroupsProperty = BindableProperty.CreateAttached(
            "VisualStateGroups", typeof(VisualStateGroupCollection), typeof(VisualStateManager), null);

        private static readonly BindablePropertyKey CurrentStatesPropertyKey = BindableProperty.CreateAttachedReadOnly(
            "CurrentStates", typeof(VisualStateCollection), typeof(VisualStateManager), null);

        public static readonly BindableProperty CurrentStatesProperty =
            CurrentStatesPropertyKey.BindableProperty;

        internal static string GetAnimationHandler(BindableObject bindable) =>
            (string) bindable.GetValue(AnimationHandlerProperty);

        public static VisualStateGroupCollection GetVisualStateGroups(BindableObject bindable)
        {
            return (VisualStateGroupCollection) bindable.GetValue(VisualStateGroupsProperty);
        }

        public static void SetVisualStateGroups(BindableObject bindable, VisualStateGroupCollection value)
        {
            bindable.SetValue(VisualStateGroupsProperty, value);
        }

        public static VisualStateCollection GetCurrentStates(BindableObject bindable)
        {
            return (VisualStateCollection) bindable.GetValue(CurrentStatesProperty);
        }

        internal static void SetCurrentStates(BindableObject bindable, VisualStateCollection value)
        {
            bindable.SetValue(CurrentStatesPropertyKey, value);
        }

        public static void GoToState(VisualElement element, string stateName, bool useTransitions)
        {
            var visualStateGroup = GetVisualStateGroups(element)
                .FirstOrDefault(x => x.States.Any(y => y.Name == stateName));

            visualStateGroup?.GoToState(element, stateName, useTransitions);
        }
    }

    public class VisualStateGroupCollection : List<VisualStateGroup>
    {
    }
}