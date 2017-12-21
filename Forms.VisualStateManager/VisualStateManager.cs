using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public class VisualStateManager : BindableObject
    {
        private static readonly BindablePropertyKey AnimationHandlerPropertyKey = BindableProperty.CreateReadOnly(
            "AnimationHandler",
            typeof(string), typeof(VisualStateManager), null,
            defaultValueCreator: (_) => $"VisualStateManager_{Guid.NewGuid():N}");

        internal static readonly BindableProperty AnimationHandlerProperty =
            AnimationHandlerPropertyKey.BindableProperty;

        public static readonly BindableProperty VisualStateGroupsProperty = BindableProperty.CreateAttached(
            "VisualStateGroups",
            typeof(VisualStateGroupCollection), typeof(VisualStateManager), null,
            defaultValueCreator: (_) => new VisualStateGroupCollection());

        public static VisualStateGroupCollection GetVisualStateGroups(BindableObject bindable) =>
            (VisualStateGroupCollection) bindable.GetValue(VisualStateGroupsProperty);

        public static void SetVisualStateGroups(BindableObject bindable, VisualStateGroupCollection value) =>
            bindable.SetValue(VisualStateGroupsProperty, value);

        public static void GoToState(VisualElement element, string stateName, bool useTransitions)
        {
            var visualStateGroup = GetVisualStateGroups(element)
                .FirstOrDefault(x => x.States.Any(y => y.Name == stateName));

            visualStateGroup?.GoToState(element, stateName, useTransitions);
        }
    }

    public class VisualStateGroupCollection : ObservableCollection<VisualStateGroup>
    {
    }
}