using OTWB.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using OTWB.Profiles;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OTWB.MyControls
{
    public sealed partial class NewProfileControl : UserControl
    {
        public event EventHandler<Profile> ProfileChosen;
        public Profile NewProfile { get; set; }

        public NewProfileControl()
        {
            this.InitializeComponent();
        }

        // Convex profile
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewProfile = new Profile(-20, 20);
            if (ProfileChosen != null)
                ProfileChosen(this, NewProfile);
        }

        //Concave profile
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           NewProfile = new ConvexArcProfile();
            if (ProfileChosen != null)
                ProfileChosen(this, NewProfile);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NewProfile = new ConcaveArcProfile();
            if (ProfileChosen != null)
                ProfileChosen(this, NewProfile);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            App.viewModel.LoadProfile();
            if (ProfileChosen != null)
                ProfileChosen(this, App.viewModel.CurrentProfile);
        }

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await App.viewModel.ImportProbeData();
            if (ProfileChosen != null)
                ProfileChosen(this, App.viewModel.CurrentProfile);
        }
    }
}
