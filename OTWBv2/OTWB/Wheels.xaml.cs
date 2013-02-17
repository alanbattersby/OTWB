using Geometric_Chuck.MyControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TCD.Controls;
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
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Geometric_Chuck
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Wheels : Geometric_Chuck.Common.LayoutAwarePage
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
                viewModel = (ViewModel)e.Parameter;
                viewModel.SetupPattern(PatternType.WHEELS, null, -1);
                viewModel.WheelsDataChanged += viewModel_WheelsDataChanged;

                this.DataContext = viewModel;
                IncrementCombo.SelectedValue = viewModel.Increment;
                PatternChoices.ItemsSource = viewModel.WheelsPatterns;
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

        private void ReCalculate()
        {
            viewModel.CreatePaths();
            WheelsPathDisplay.CurrentPath = viewModel.CurrentPath;
            WheelsPathDisplay.ShowPaths();
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
            viewModel.ImportPattern(PatternType.WHEELS);
            PatternChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = WheelsPathDisplay.Grid;
            Flyout f = new Flyout(
              new SolidColorBrush(Colors.White),//the foreground color of all flyouts
              (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
              new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
              "Change Grid Size",
              FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
              gc);
            f.DataContext = WheelsPathDisplay.Grid;
            f.Name = "GridControl";
            f.OnClosing += f_OnClosing;
            f.ShowAsync();
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            PointsControl pc = new PointsControl();
            pc.DataContext = WheelsPathDisplay.CurrentPath.AllPoints;
            pc.SelectedPath = 0;
            Flyout f = new Flyout(
             new SolidColorBrush(Colors.White),//the foreground color of all flyouts
             (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
             new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
             "Points",
             FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
             pc);
            f.ShowAsync();
        }

        void f_OnClosing(object sender, CloseReason reason, System.ComponentModel.CancelEventArgs cancelEventArgs)
        {
            //BazelyPathDisplay.UpdateGrid();
            ReCalculate();
        }

        private void Gcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GcodePage), viewModel);
            }
        }
    }
}
