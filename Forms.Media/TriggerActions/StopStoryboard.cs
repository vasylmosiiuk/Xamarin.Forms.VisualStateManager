using Forms.Media.Extensions;
using Xamarin.Forms;

namespace Forms.Media.TriggerActions
{
    public class StopStoryboard : TriggerAction<VisualElement>
    {
        public Storyboard Storyboard { get; set; }
        protected override void Invoke(VisualElement target)
        {
            Storyboard?.Stop(target);
        }
    }
}