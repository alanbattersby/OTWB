using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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
using Windows.System.Threading;
using OTWB.MyControls;
using System.Diagnostics;
using OTWB.PathGenerators;
using Callisto.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace OTWB
{
     
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : OTWB.Common.LayoutAwarePage
    {
        private ViewModel viewModel;
   
        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage,
        };

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
    
            PatternChoices.SelectionChanged += PatternChoices_SelectionChanged;
          
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //viewModel = (ViewModel)e.Parameter;
            viewModel = App.viewModel;
            viewModel.SetupPattern(PatternType.bazley, null, -1);
            viewModel.CurrentPathData.PropertyChanged += CurrentPathData_PropertyChanged;
            this.DataContext = viewModel;
            IncrementCombo.SelectedValue = viewModel.Increment;

            PatternChoices.ItemsSource = viewModel.BazeleyPatterns;
            PatternChoices.SelectedIndex = viewModel.CurrentPathData.PatternIndex;     
            PatternChoices.SelectionChanged += PatternChoices_SelectionChanged;

            ProgressRing1.IsActive = false;
        }

        void CurrentPathData_PropertyChanged(object sender, EventArgs e)
        {
            ReCalculate();
        }


        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            viewModel.CurrentPathData.PropertyChanged -= CurrentPathData_PropertyChanged;
            //viewModel.CleanUpForPageChange();
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
            //string showpt = resourceLoader.GetString("SHOW_POINT");
            //if (pageState != null && pageState.ContainsKey(showpt))
            //    SHOW_POINT = (bool)pageState[showpt];
            //else
            //    SHOW_POINT = true;

            //string patternData = resourceLoader.GetString("BAZELY_PATTERNS");
            //if (pageState != null && pageState.ContainsKey(patternData))
            //{
            //    _PatternData = (ObservableCollection<BazelyChuck>)pageState[patternData];
            //    PatternChoices.ItemsSource = _PatternData;
            //}

            //string current_pattern = resourceLoader.GetString("CURRENT_BAZELY_PATTERN");
            //int current = 0;
            //if (pageState != null && pageState.ContainsKey(current_pattern) )
            //{
            //    current = (int)pageState[current_pattern];
            //}
            //if (PatternChoices.Items.Count > current )
            //    PatternChoices.SelectedIndex = current;
            //if (pageState["PatternData"] != null)
            //    _PatternData = ( ObservableCollection<BazelyChuck>)pageState["PatternData"];
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            //if (_PatternData != null)
            //    pageState["PatternData"] = _PatternData;
            //var resourceLoader = new ResourceLoader();
            //string showpt = resourceLoader.GetString("SHOW_POINT");
            //pageState[showpt] = SHOW_POINT;
            //string current_pattern = resourceLoader.GetString("CURRENT_BAZELY_PATTERN");
            //pageState[current_pattern] = PatternChoices.SelectedIndex;

            //string patternData = resourceLoader.GetString("BAZELY_PATTERNS");
            //pageState[patternData] = _PatternData;
        }

        private void ProgressBarVisible(bool visible)
        {
            ProgressRing1.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRing1.IsActive = visible;
        }

        private async void GeneratePaths()
        {
            ProgressBarVisible(true);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                viewModel.CreatePaths();
                BazleyPathDisplay.CurrentPath = viewModel.CurrentPath;
                BazleyPathDisplay.ShowPaths();
            });
            ProgressBarVisible(false);
        }


        private void ReCalculate()
        {
            GeneratePaths();
        }

        private void IncrementCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncrementCombo.SelectedValue != null)
            {
                viewModel.Increment = (double)IncrementCombo.SelectedValue;
                ReCalculate();
            }
        }

        private void PatternChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatternChoices.SelectedValue != null)
            {
                BazelyChuck bc = (BazelyChuck)PatternChoices.SelectedValue;
                viewModel.CurrentPathData = bc;
                viewModel.CurrentPathData.PropertyChanged += CurrentPathData_PropertyChanged;
                ReCalculate();
            }
        }
     
         private async void Export_Path_Click(object sender, RoutedEventArgs e)
        {
            // test to export all patterns to an xml file
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
                NotifyUser("Cannot unsnap the sample.", NotifyType.StatusMessage);
            }

            return unsnapped;
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                // Use the status message style.
                case NotifyType.StatusMessage:
                    StatusBlock.Style = Resources["StatusStyle"] as Style;
                    break;
                // Use the error message style.
                case NotifyType.ErrorMessage:
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

      
        private void bsd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ReCalculate();
        }

             
        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = BazleyPathDisplay.Grid;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(BazleyPathDisplay.Grid.Foreground);
            settings.HeaderText = "Change Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = BazleyPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (BazleyPathDisplay.CurrentPath.Count == 0) return;
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
            settings.ContentBackgroundBrush = new SolidColorBrush(Colors.Black);
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;

        }

        private void Save_Pattern_Click(object sender, RoutedEventArgs e)
        {
            if (this.EnsureUnsnapped())
            {
                viewModel.ExportCurrentPatternData();
            }
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
