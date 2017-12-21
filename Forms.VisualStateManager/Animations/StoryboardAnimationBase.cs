using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public abstract class StoryboardAnimationBase : IStoryboardAnimation
    {
        public TimeSpan BeginTime { get; set; }
        public Duration Duration { get; set; } = new Duration(TimeSpan.Zero);
        public BindableProperty TargetProperty { get; set; }
        public VisualElement Target { get; set; }
        public bool RepeatBehavior { get; set; }
        public bool AutoReverse { get; set; }
        public abstract void Update(double x);
    }
}