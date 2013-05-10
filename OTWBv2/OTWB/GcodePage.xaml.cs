using OTWB.MyControls;
using Callisto.Controls;
using OTWB.Coordinates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Imaging;
using OTWB.Settings;
using System.Diagnostics;
using Windows.Storage.Pickers;
using OTWB.CodeGeneration;
using OTWB.Common;
using Windows.UI.Core;
using System.Text;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace OTWB
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GcodePage : OTWB.Common.LayoutAwarePage
    {
        ViewModel viewModel;
        CodeGenViewModel codeGen;

        public GcodePage()
        {
            this.InitializeComponent();
            codeGen = new CodeGenViewModel();
            //CodeViewer.DataContext = codeGen.Code;
            DataContext = codeGen;
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel = App.viewModel;

            if ((viewModel.CurrentPath == null) || (viewModel.CurrentPath.Count == 0))
            {
                await codeGen.ImportPath();
                GenerateGcode();
            }
            else
            {
                codeGen.ToolPaths = viewModel.CurrentPath;
                codeGen.CurrentPath = viewModel.CurrentPathAsListofPoint;
                codeGen.PathName = viewModel.CurrentPath.PatternName;
                codeGen.Profile = viewModel.CurrentProfile;
                GenerateGcode();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //SettingsPane.GetForCurrentView().CommandsRequested -= GcodePage_CommandsRequested;
            base.OnNavigatedFrom(e);
            
        }

        //private void GcodePage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        //{
        //    SettingsCommand cmd = new SettingsCommand("GCode", "Code Generation", (x) =>
        //    {
        //        // create a new instance of the flyout
        //        SettingsFlyout settings = new SettingsFlyout();
        //        // set the desired width.  If you leave this out, you will get Narrow (346px)
        //        settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;

        //        // optionally change header and content background colors away from defaults (recommended)
        //        // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
        //        // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
        //        settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
        //        settings.HeaderText = "G-Code"; // string.Format("{0}", App.VisualElements.DisplayName);

        //        // provide some logo (preferrably the smallogo the app uses)
        //        BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
        //        settings.SmallLogoImageSource = bmp;

        //        // set the content for the flyout
        //        CodeSettingsContent c = new CodeSettingsContent();
        //        //c.DataContext = App.CodeSettingsContext;
        //        settings.Content = c;
        //        // open it
        //        settings.IsOpen = true;

        //    });

        //    args.Request.ApplicationCommands.Add(cmd);
        //}
     
        private void Save_Code_Click(object sender, RoutedEventArgs e)
        {
            if (codeGen.Code.Count == 0) return;
            if (this.EnsureUnsnapped())
            {
                codeGen.SaveCode();
            }
        }

        private bool EnsureUnsnapped()
        {
            // FilePicker APIs will not work if the application is in a snapped state.
            // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            if (!unsnapped)
            {
                //NotifyUser("Cannot unsnap the sample.", NotifyType.StatusMessage);
            }

            return unsnapped;
        }


        private void Points_Click(object sender, RoutedEventArgs e)
        {

            if ((codeGen.CurrentPath == null) || (codeGen.CurrentPath.Count == 0)) return;
           
            PointsControl pc = new PointsControl();
            pc.DataContext = viewModel.CurrentPath.AllPaths; 
            pc.SelectedPath = 0;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            settings.HeaderText = "Points"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;
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

        private void ProgressBarVisible(bool visible)
        {
            CodeProgress.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            CodeProgress.IsActive = visible;
        }

        private async void GenerateGcode()
        {
            ProgressBarVisible(true);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                codeGen.GenerateCode();
            });
            ProgressBarVisible(false);
        }

        private void Generate_Code_Click(object sender, RoutedEventArgs e)
        {
            GenerateGcode();
        }

        private void Clear_Code_Click(object sender, RoutedEventArgs e)
        {
            codeGen.Clear();
        }
    
    }
}
