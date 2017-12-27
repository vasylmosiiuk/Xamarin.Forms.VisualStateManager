using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Animations
{
    public abstract class StoryboardAnimationBase : IStoryboardAnimation
    {
        public FillBehavior FillBehavior { get; set; }
        public Easing Easing { get; set; }
        public TimeSpan BeginTime { get; set; }
        public Duration Duration { get; set; } = new Duration(TimeSpan.Zero);
        public BindableProperty TargetProperty { get; set; }
        public RepeatBehavior RepeatBehavior { get; set; }
        public bool AutoReverse { get; set; }
        public virtual object Prepare(VisualElement target) => null;

        public abstract void Update(double x, object state);

        public virtual void Cleanup(object state)
        {
        }
    }
}