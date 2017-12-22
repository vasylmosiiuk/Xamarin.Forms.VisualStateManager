using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public interface IStoryboardAnimation
    {
        BindableProperty TargetProperty { get; set; }
        Easing Easing { get; set; }
        TimeSpan BeginTime { get; set; }
        Duration Duration { get; set; }
        VisualElement Target { get; set; }
        RepeatBehavior RepeatBehavior { get; set; }
        bool AutoReverse { get; set; }
        void Update(double x);
    }
}