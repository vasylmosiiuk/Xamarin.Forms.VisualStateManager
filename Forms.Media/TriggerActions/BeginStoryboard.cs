using Forms.Media.Extensions;
using Xamarin.Forms;

namespace Forms.Media.TriggerActions
{
    public class BeginStoryboard : TriggerAction<VisualElement>
    {
        public Storyboard Storyboard { get; set; }
        protected override void Invoke(VisualElement target)
        {
            Storyboard?.Begin(target);
        }
    }
}
