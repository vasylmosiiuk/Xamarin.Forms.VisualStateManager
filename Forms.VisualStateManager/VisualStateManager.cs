using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public class VisualStateManager : BindableObject
    {
        public static readonly BindableProperty VisualStateGroupsProperty = BindableProperty.CreateAttached(
            "VisualStateGroups",
            typeof(VisualStateGroupCollection), typeof(VisualStateManager), null,
            defaultValueCreator: (_) => new VisualStateGroupCollection());

        public static VisualStateGroupCollection GetVisualStateGroups(BindableObject bindable) =>
            (VisualStateGroupCollection) bindable.GetValue(VisualStateGroupsProperty);

        public static void SetVisualStateGroups(BindableObject bindable, VisualStateGroupCollection value) =>
            bindable.SetValue(VisualStateGroupsProperty, value);
    }

    public class VisualStateGroupCollection : ObservableCollection<VisualStateGroup>
    {
    }
}