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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //VisualStateManager.GoToState(Frame, "a1", false);
        }

        private void OnStateSwitchButtonClicked(object sender, EventArgs e)
        {
            var currentState = VisualStateManager.GetCurrentStates(Frame1)?.FirstOrDefault();
            var currentStateName = currentState?.Name;
            var stateNameToSet = currentStateName == "a1" ? "a2" : "a1";
            VisualStateManager.GoToState(Frame1, stateNameToSet, true);
        }

        private void OnStateSwitchButtonClicked2(object sender, EventArgs e)
        {
            var currentState = VisualStateManager.GetCurrentStates(Frame3)?.FirstOrDefault();
            var currentStateName = currentState?.Name;
            var stateNameToSet = currentStateName == "a1" ? "a2" : "a1";

            VisualStateManager.GoToState(Frame3, stateNameToSet, true);
        }
    }
}