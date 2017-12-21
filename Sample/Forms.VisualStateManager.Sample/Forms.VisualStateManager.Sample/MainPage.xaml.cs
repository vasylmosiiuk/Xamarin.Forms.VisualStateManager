using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager.Sample
{
    public partial class MainPage : ContentPage
    {
        public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(nameof(CurrentState),
            typeof(VisualState), typeof(MainPage), default(VisualState));

        public MainPage()
        {
            InitializeComponent();
        }

        public VisualState CurrentState
        {
            get => (VisualState) GetValue(CurrentStateProperty);
            set => SetValue(CurrentStateProperty, value);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            VisualStateManager.GoToState(Frame, "a1", false);
        }

        private void OnStateSwitchButtonClicked(object sender, EventArgs e)
        {
            var currentStateName = CurrentState?.Name;
            var stateNameToSet = currentStateName == "a1" ? "a2" : "a1";
            VisualStateManager.GoToState(Frame, stateNameToSet, false);
        }
    }
}