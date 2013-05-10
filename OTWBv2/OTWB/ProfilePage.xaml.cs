using Callisto.Controls;
using OTWB.Interfaces;
using OTWB.MyControls;
using OTWB.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace OTWB
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ProfilePage : OTWB.Common.LayoutAwarePage
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Save_Profile_Click(object sender, RoutedEventArgs e)
        {
            App.viewModel.SaveProfile();
        }

        private void Load_Profile_Click(object sender, RoutedEventArgs e)
        {
            App.viewModel.LoadProfile();
        }

        private void New_Profile_Click(object sender, RoutedEventArgs e)
        {
            NewProfileControl nc = new NewProfileControl();
            nc.ProfileChosen += nc_ProfileChosen;

            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            settings.HeaderText = "Profiles"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = ProfileDisplayControl.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = nc;
            // open it
            settings.IsOpen = true;
        }

        void nc_ProfileChosen(object sender, Profile e)
        {
            App.viewModel.CurrentProfile = e;
            ProfileDisplayControl.Profile = e;
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            CartesianGridControl gc = new CartesianGridControl();
            gc.DataContext = ProfileDisplayControl.Grid;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(ProfileDisplayControl.Grid.Foreground);
            settings.HeaderText = "Change Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = ProfileDisplayControl.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
        }
    }
}
