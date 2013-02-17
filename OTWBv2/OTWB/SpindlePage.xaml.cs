using Geometric_Chuck.Interfaces;
using Geometric_Chuck.MyControls;
using Geometric_Chuck.Spindle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TCD.Controls;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Geometric_Chuck
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SpindlePage : Geometric_Chuck.Common.LayoutAwarePage
    {
        ViewModel viewModel;
        public SpindlePage()
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

        private void Add_Rosette_Click(object sender, RoutedEventArgs e)
        {
            // Create a flyout to specify rosette type
            Flyout f = new Flyout(
             new SolidColorBrush(Colors.White),//the foreground color of all flyouts
             (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
             new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
             "New Rosette",
             FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
             new NewRosetteFlyoutControl());
            f.OnClosing += f_OnClosing;
            f.ShowAsync();
        }

        void f_OnClosing(object sender, CloseReason reason, System.ComponentModel.CancelEventArgs cancelEventArgs)
        {
            if (sender is Flyout)
            {
                if (reason == CloseReason.BackButton)
                {
                    if (!cancelEventArgs.Cancel)
                    {
                        NewRosetteFlyoutControl c = (NewRosetteFlyoutControl)(sender as Flyout).Content;
                        Rosette ros = c.NewRosette;
                        if ((viewModel.CurrentPathData != null) &&
                            (viewModel.CurrentPathData is Barrel))
                            (viewModel.CurrentPathData as Barrel).Add(ros);
                        else
                        {

                        }
                    }
                }
                ReCalculate();
            }
        }

        private void Remove_Rosette_click(object sender, RoutedEventArgs e)
        {
            Rosette sd = SpindleData.CurrentRosette;
            (viewModel.CurrentPathData as Barrel).Rosettes.Remove(sd);
            ReCalculate();
        }

        private void ReCalculate()
        {
            viewModel.CreatePaths();
            BarrelPathDisplay.CurrentPath = viewModel.CurrentPath;
            BarrelPathDisplay.ShowPaths();

        }

        private async void ExportPaths_Click(object sender, RoutedEventArgs e)
        {
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
            //if (!unsnapped)
            //{
            //    NotifyUser("Cannot unsnap the sample.", NotifyType.StatusMessage);
            //}

            return unsnapped;
        }
        private void Export_Spindle_Click(object sender, RoutedEventArgs e)
        {
            if (this.EnsureUnsnapped())
            {
                viewModel.ExportCurrentPatternData();
            }
        }

        private void Import_Spindle_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ImportPattern(PatternType.BARREL);
            BarrelChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private void Clear_Barrel_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentPathData is Barrel)
                (viewModel.CurrentPathData as Barrel).Rosettes.Clear();
            ReCalculate();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            BarrelPathDisplay.CleanUp();
            //viewModel.CleanUpForPageChange();
            this.viewModel = null;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel = (ViewModel)e.Parameter;
            this.DataContext = viewModel;
            viewModel.SetupPattern(PatternType.BARREL, null, -1);

            BarrelChoices.SelectedIndex = viewModel.SelectedPathIndex;
            BarrelChoices.SelectionChanged += BarrelChoices_SelectionChanged;
            BarrelChoices.SelectedItem = viewModel.BarrelPatterns[0];
            ReCalculate();
        }

        private void BarrelChoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BarrelChoices.SelectedValue != null)
            {
                Barrel choice = (Barrel)BarrelChoices.SelectedValue;
                comboBox3.SetUpItems(choice.CombinationRule);
                choice.PropertyChanged += choice_PropertyChanged;
                viewModel.CurrentPathData = choice;
                  
                ReCalculate();
            }
        }

        void choice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           //Debug.WriteLine("SpindlePage prop {0} changed", e.PropertyName);
           ReCalculate();
        }

        private void BarrelPhaseSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (viewModel.CurrentPathData is Barrel)
            {
                (viewModel.CurrentPathData as Barrel).Phase = e.NewValue;
                ReCalculate();
            }
        }

        private void IncrementCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IncrementCombo.SelectedValue != null)
            {
                viewModel.Increment = (double)IncrementCombo.SelectedValue;
                ReCalculate();
            }
        }

        private void New_Barrel_Click(object sender, RoutedEventArgs e)
        {
            int nxtfree = viewModel.BarrelPatterns.Count;
            viewModel.BarrelPatterns.Add(new Barrel(nxtfree));
            BarrelChoices.SelectedIndex = nxtfree;
        }

        private void ToolSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (viewModel != null)
                if (viewModel.CurrentPathData is Barrel)
                {
                    (viewModel.CurrentPathData as Barrel).ToolPosition = e.NewValue;
                }
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = BarrelPathDisplay.Grid;
            Flyout f = new Flyout(
              new SolidColorBrush(Colors.White),//the foreground color of all flyouts
              (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
              new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
              "Change Grid Size",
              FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
              gc);
            f.DataContext = BarrelPathDisplay.Grid;
            f.Name = "GridControl";
            f.OnClosing += g_OnClosing;
            f.ShowAsync();
        }

        void g_OnClosing(object sender, CloseReason reason, System.ComponentModel.CancelEventArgs cancelEventArgs)
        {
            //BazelyPathDisplay.UpdateGrid();
            ReCalculate();
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (BarrelPathDisplay.CurrentPath.Count == 0) return;
            PointsControl pc = new PointsControl();
            pc.DataContext = BarrelPathDisplay.CurrentPath.AllPoints;
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

        private void Gcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GcodePage), viewModel);
            }
        }

       
    }
}
