using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public interface IStoryboardAnimation
    {
        BindableProperty TargetProperty { get; set; }
        FillBehavior FillBehavior { get; set; }
        Easing Easing { get; set; }
        TimeSpan BeginTime { get; set; }
        Duration Duration { get; set; }
        RepeatBehavior RepeatBehavior { get; set; }
        bool AutoReverse { get; set; }
        object StoreVisualState(VisualElement target);
        void Update(double x, object state);
        void RestoreVisualState(VisualElement target, object state);
    }
}