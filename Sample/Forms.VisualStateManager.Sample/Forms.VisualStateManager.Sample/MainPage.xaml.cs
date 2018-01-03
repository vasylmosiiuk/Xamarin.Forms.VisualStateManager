using System;
using System.Linq;
using System.Threading.Tasks;
using Forms.Media.VisualStateManager;
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

            Media.VisualStateManager.VisualStateManager.GoToState(Header, "Hidden", false);
        }

        public static readonly BindableProperty CurrentHeaderStateProperty = BindableProperty.Create(nameof(CurrentHeaderState), typeof(VisualState), typeof(MainPage), default(VisualState));

        public VisualState CurrentHeaderState
        {
            get => (VisualState)GetValue(CurrentHeaderStateProperty);
            set => SetValue(CurrentHeaderStateProperty, value);
        }


        private void OnTapped(object sender, EventArgs e)
        {
            var stateToSet = CurrentHeaderState?.Name == "Hidden" ? "Visible" : "Hidden";
            Media.VisualStateManager.VisualStateManager.GoToState(Header, stateToSet, true);
        }
    }
}