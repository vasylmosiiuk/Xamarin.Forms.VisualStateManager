using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public abstract class StoryboardAnimationBase : IStoryboardAnimation
    {
        public Easing Easing { get; set; }
        public TimeSpan BeginTime { get; set; }
        public Duration Duration { get; set; } = new Duration(TimeSpan.Zero);
        public BindableProperty TargetProperty { get; set; }
        public RepeatBehavior RepeatBehavior { get; set; }
        public bool AutoReverse { get; set; }
        public abstract void Update(VisualElement target, double x);
    }
}