using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    [ContentProperty(nameof(Animations))]
    public sealed class Storyboard : BindableObject
    {
        private Duration _duration = Duration.Automatic;

        public Duration Duration
        {
            get
            {
                if (_duration == Duration.Automatic)
                    return new Duration(Animations.Max(x =>
                        x.CalculateExactAnimationDuration().ToTimeSpan().SumSafety(x.BeginTime)));

                return _duration;
            }
            set => _duration = value;
        }

        public Easing Easing { get; set; } = Easing.Linear;
        public IList<IStoryboardAnimation> Animations { get; } = new List<IStoryboardAnimation>();
        public VisualElement Target { get; set; }
    }
}