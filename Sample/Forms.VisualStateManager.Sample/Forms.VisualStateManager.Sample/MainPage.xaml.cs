using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Forms.VisualStateManager.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnChangeStateButtonClick(object sender, EventArgs e)
        {
            var root = (VisualElement)((VisualElement) sender).Parent;
            var groups = VisualStateManager.GetVisualStateGroups(root);

            if (groups?.Any() ?? false)
            {
                var currentAStateName = groups.FirstOrDefault(x => x.Name.StartsWith("a"))?.CurrentState?.Name;
                var currentBStateName = groups.FirstOrDefault(x => x.Name.StartsWith("b"))?.CurrentState?.Name;
                var stateNameToSet1 = currentAStateName == "a1" ? "a2" : "a1";
                var stateNameToSet2 = currentBStateName == "b1" ? "b2" : "b1";
                VisualStateManager.GoToState(root, stateNameToSet1, true);
                VisualStateManager.GoToState(root, stateNameToSet2, true);
            }
        }
    }
}