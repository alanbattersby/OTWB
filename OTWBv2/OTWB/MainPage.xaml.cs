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
using TCD.Controls;
using Geometric_Chuck.MyControls;
using System.Diagnostics;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Geometric_Chuck
{
     
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Geometric_Chuck.Common.LayoutAwarePage
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
            viewModel = (ViewModel)e.Parameter;
            viewModel.SetupPattern(PatternType.BAZELEY, null, -1);
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

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
            
        //    if (this.Frame != null)
        //    {
        //        viewModel.CleanUpForPageChange(true);
        //        this.Frame.Navigate(typeof(RossPage), viewModel);
        //    }
        //}

        private void ReCalculate()
        {
            viewModel.CreatePaths();
            BazelyPathDisplay.CurrentPath = viewModel.CurrentPath;
            BazelyPathDisplay.ShowPaths();
            //PointsView.DataContext = viewModel.CurrentPath;
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

      
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Edit the stage selected in the viewer
            BazelyStageData bsd = (BazelyStageData)BazelyStageList.SelectedItem;
            bsd.PropertyChanged+=bsd_PropertyChanged;
            BSDControl s = new BSDControl();
            s.DataContext = bsd;
            Flyout f = new Flyout(
               new SolidColorBrush(Colors.White),//the foreground color of all flyouts
               (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
               new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
               "Stage Data Editor",
               FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
               s);
            f.OnClosing += f_OnClosing;
            f.ShowAsync();
        }

        private void bsd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ReCalculate();
        }

        void f_OnClosing(object sender, CloseReason reason, System.ComponentModel.CancelEventArgs cancelEventArgs)
        {
            //BazelyPathDisplay.UpdateGrid();
            ReCalculate();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = BazelyPathDisplay.Grid;
            Flyout f = new Flyout(
              new SolidColorBrush(Colors.White),//the foreground color of all flyouts
              (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
              new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
              "Change Grid Size",
              FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
              gc);
            f.DataContext = BazelyPathDisplay.Grid;
            f.Name = "GridControl";
            f.OnClosing += f_OnClosing;
            f.ShowAsync();
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (BazelyPathDisplay.CurrentPath.Count == 0) return;
            PointsControl pc = new PointsControl();
            pc.DataContext = BazelyPathDisplay.CurrentPath.AllPoints;
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
                this.Frame.Navigate(typeof(GcodePage), viewModel);
            }
        }

    }
}
