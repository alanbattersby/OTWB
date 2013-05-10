using Callisto.Controls;
using OTWB.MyControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI;
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
    public sealed partial class Wheels : OTWB.Common.LayoutAwarePage
    {
        ViewModel viewModel;

        public Wheels()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                //viewModel = (ViewModel)e.Parameter;
                viewModel = App.viewModel;
                viewModel.SetupPattern(PatternType.wheels, null, -1);
                viewModel.WheelsDataChanged += viewModel_WheelsDataChanged;

                this.DataContext = viewModel;
                IncrementCombo.SelectedValue = viewModel.Increment;
                PatternChoices.ItemsSource = viewModel.WheelsPatterns;
                if (viewModel.CurrentPathData != null)
                    PatternChoices.SelectedIndex = viewModel.CurrentPathData.PatternIndex;


                PatternChoices.SelectionChanged += PatternChoices_SelectionChanged;

                ProgressRing1.IsActive = false;
                ReCalculate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        void viewModel_WheelsDataChanged(object sender, EventArgs e)
        {
            ReCalculate();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            viewModel.WheelsDataChanged -= viewModel_WheelsDataChanged;
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

        private void ExportPaths_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private async void SaveData()
        {
            await viewModel.ExportCurrentPath();
        }

        private void IncrementCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncrementCombo.SelectedValue != null)
            {
                viewModel.Increment = (double)IncrementCombo.SelectedValue;
                ReCalculate();
            }
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
                WheelsPathDisplay.CurrentPath = viewModel.CurrentPath;
                WheelsPathDisplay.ShowPaths();
            });
            ProgressBarVisible(false);
        }

        private void ReCalculate()
        {
            GeneratePaths();
        }

        private void PatternChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatternChoices.SelectedValue != null)
            {
                WheelsData bc = (WheelsData)PatternChoices.SelectedValue;
                viewModel.CurrentPathData = bc;
                ReCalculate();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Export current PatternData as XML file
            viewModel.ExportCurrentPatternData();
        }

        private void Add_Wheel_Click(object sender, RoutedEventArgs e)
        {
            // add another wheel to current data 
            WheelsData wd = (WheelsData)PatternChoices.SelectedValue;
            wd.Add(1, 1);
            ReCalculate();
        }

        private void Remove_Wheel_click(object sender, RoutedEventArgs e)
        {
            WheelStageData sd = (WheelStageData)WheelsDataList.SelectedItem;
            (viewModel.CurrentPathData as WheelsData).Stages.Remove(sd);
            ReCalculate();
        }

        private void Import_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ImportPattern(PatternType.wheels);
            PatternChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = WheelsPathDisplay.Grid;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(WheelsPathDisplay.Grid.Foreground);
            settings.HeaderText = "Change Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = WheelsPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (WheelsPathDisplay.CurrentPath.Count == 0) return;
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
            settings.ContentBackgroundBrush = WheelsPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;

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
