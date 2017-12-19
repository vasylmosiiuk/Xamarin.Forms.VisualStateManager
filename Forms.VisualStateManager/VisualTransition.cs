using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    [ContentProperty(nameof(Storyboard))]
    public class VisualTransition : BindableObject
    {
        public static readonly BindableProperty StoryboardProperty =
            BindableProperty.Create(nameof(Storyboard), typeof(Storyboard), typeof(VisualTransition),
                default(Storyboard), coerceValue: EnsureStoryboardCorrect);

        public static readonly BindableProperty FromProperty =
            BindableProperty.Create(nameof(From), typeof(string), typeof(VisualTransition), default(string));


        public static readonly BindableProperty ToProperty =
            BindableProperty.Create(nameof(To), typeof(string), typeof(VisualTransition), default(string));
        
        public Storyboard Storyboard
        {
            get => (Storyboard) GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        public string From
        {
            get => (string) GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public string To
        {
            get => (string) GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }
        
        private static object EnsureStoryboardCorrect(BindableObject bindable, object value)
        {
            return value ?? new Storyboard();
        }
    }
}