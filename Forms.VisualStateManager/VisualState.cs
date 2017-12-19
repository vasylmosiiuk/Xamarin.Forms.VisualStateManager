using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    [ContentProperty(nameof(Storyboard))]
    public sealed class VisualState : BindableObject
    {
        public static readonly BindableProperty StoryboardProperty =
            BindableProperty.Create(nameof(Storyboard), typeof(Storyboard), typeof(VisualState));

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(VisualState), null);

        public Storyboard Storyboard
        {
            get => (Storyboard) GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }
    }
}