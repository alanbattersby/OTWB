using Callisto.Controls;
using OTWB.Interfaces;
using OTWB.MyControls;
using OTWB.PathGenerators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace OTWB
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RossPage : OTWB.Common.LayoutAwarePage
    {
        private ViewModel viewModel;

        private const double ToRadians = Math.PI / 180;
        private const double Alpha = ToRadians * 360;
        private const double ToDegrees = 1 / ToRadians;

        public RossPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            RossPatternChoices.SelectionChanged += PatternChoices_SelectionChanged;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            RossPathDisplay.CleanUp();
            //viewModel.CleanUpForPageChange();
            this.viewModel = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //viewModel = (ViewModel)e.Parameter;
            viewModel = App.viewModel;
            viewModel.SetupPattern(PatternType.ross, null, -1);
            this.DataContext = viewModel;
            RossPatternChoices.ItemsSource = viewModel.RossPatterns;
            RossIncrementCombo.SelectedValue = viewModel.Increment;
            RossPatternChoices.SelectedIndex = viewModel.CurrentPathData.PatternIndex;
            RossPatternChoices.SelectionChanged += PatternChoices_SelectionChanged;
            ReCalculate();
        }

        void RossIncrementCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RossPatternChoices.SelectedValue != null)
            {
                viewModel.Increment = (double)RossIncrementCombo.SelectedValue;
                ReCalculate();
            }
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
            //var resourceLoader = new ResourceLoader();
            //string showpt = resourceLoader.GetString("ROSS_SHOW_POINT");
            //if (pageState != null && pageState.ContainsKey(showpt))
            //    ROSS_SHOW_POINT = (bool)pageState[showpt];
            //else
            //    ROSS_SHOW_POINT = true;

            //string patternData = resourceLoader.GetString("ROSS_PATTERNS");
            //if (pageState != null && pageState.ContainsKey(patternData))
            //{
            //    _PatternData = (ObservableCollection<RossData>)pageState[patternData];
            //    RossPatternChoices.ItemsSource = _PatternData;
            //}

            //string current_pattern = resourceLoader.GetString("CURRENT_ROSS_PATTERN");
            //int current = 0;
            //if (pageState != null && pageState.ContainsKey(current_pattern))
            //{
            //    current = (int)pageState[current_pattern];
            //}
            //if (RossPatternChoices.Items.Count > current)
            //    RossPatternChoices.SelectedIndex = current;
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

        private void PatternChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Debug.WriteLine("PatternChoices selection changed to {0}",((RossData)RossPatternChoices.SelectedValue).Name);
            if (RossPatternChoices.SelectedValue != null)
            {
                viewModel.CurrentPathData = (Interfaces.PathData)RossPatternChoices.SelectedValue;
                viewModel.CurrentPathData.PropertyChanged += CurrentPathData_PropertyChanged;
                ReCalculate();
            }
        }

        void CurrentPathData_PropertyChanged(object sender, EventArgs e)
        {
            ReCalculate();
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRing2.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRing2.IsActive = visible;
        }


        private async void GeneratePaths()
        {
            ProgressBarVisible(true);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                viewModel.CreatePaths();
                RossPathDisplay.CurrentPath = viewModel.CurrentPath;
                RossPathDisplay.ShowPaths();
            });
            ProgressBarVisible(false);
        }

        private void ReCalculate()
        {
            GeneratePaths();
            //viewModel.CreatePaths();
            //RossPathDisplay.CurrentPath = viewModel.CurrentPath;
            //RossPathDisplay.ShowPaths();
            //PointsView.DataContext = viewModel.CurrentPath;
        }

        private void IncrementCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.Increment = (double)RossIncrementCombo.SelectedValue;
            ReCalculate();
        }


        private async void ExportPaths_Click(object sender, RoutedEventArgs e)
        {
            // this will export the path list of the current pattern
            if (this.EnsureUnsnapped())
            {
                await viewModel.ExportCurrentPath();
            }
        }

        internal bool EnsureUnsnapped()
        {
            // FilePicker APIs will not work if the application is in a snapped state.
            // If an app wants to show a FilePicker while snapped, it must attempt to unsnap first
            bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
            if (!unsnapped)
            {
                NotifyUser("Cannot unsnap the sample.", OTWB.MainPage.NotifyType.StatusMessage);
            }

            return unsnapped;
        }

        public void NotifyUser(string strMessage, OTWB.MainPage.NotifyType type)
        {
            switch (type)
            {
                // Use the status message style.
                case OTWB.MainPage.NotifyType.StatusMessage:
                    StatusBlock.Style = Resources["StatusStyle"] as Style;
                    break;
                // Use the error message style.
                case OTWB.MainPage.NotifyType.ErrorMessage:
                    StatusBlock.Style = Resources["ErrorStyle"] as Style;
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void HomeViewButton_Click(object sender, RoutedEventArgs e)
        {
            RossPathDisplay.Home();
        }

        private void CentreViewButton_Click_1(object sender, RoutedEventArgs e)
        {
            RossPathDisplay.Centre();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = RossPathDisplay.Grid;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(RossPathDisplay.Grid.Foreground);
            settings.HeaderText = "Change Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = RossPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
        }

      
        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (RossPathDisplay.CurrentPath.Count == 0) return;
            PointsControl pc = new PointsControl();
            pc.DataContext = viewModel.CurrentPath.AllPaths; //BazleyPathDisplay.CurrentPath.AllPoints;
            pc.SelectedPath = 0;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            settings.HeaderText = "Points"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = RossPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;

        }

        private void Export_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ExportCurrentPatternData();
        }

        private void Import_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ImportPattern(PatternType.ross);
            RossPatternChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private void Gcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GcodePage));
            }
        }
    }
}
