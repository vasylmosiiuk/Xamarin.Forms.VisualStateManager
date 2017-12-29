using System;
using System.Collections.Generic;
using System.Linq;
using Forms.VisualStateManager.Helpers;
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
            "VisualStateGroups", typeof(VisualStateGroupCollection), typeof(VisualStateManager), null, propertyChanged: OnVisualStateGroupsChanged);

        private static void OnVisualStateGroupsChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue is VisualStateGroupCollection visualStateGroups)
            {
                foreach (var visualStateGroup in visualStateGroups)
                    visualStateGroup.ApplySafety();
            }
        }

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

        public static void GoToState(VisualElement element, string stateName, bool useTransitions)
        {
            var target = element;
            if (element is TemplatedView templatedView && templatedView.ControlTemplate != null)
            {
                var controlTemplateRoot = templatedView.Children.FirstOrDefault() as VisualElement;
                if (GetVisualStateGroups(controlTemplateRoot) != null)
                    target = controlTemplateRoot;
            }

            var visualStateGroup = GetVisualStateGroups(target)
                .FirstOrDefault(x => x.States.Any(y => y.Name == stateName));

            visualStateGroup?.GoToState(element, target, stateName, useTransitions);
        }
    }

    public class VisualStateGroupCollection : List<VisualStateGroup>
    {
    }
}