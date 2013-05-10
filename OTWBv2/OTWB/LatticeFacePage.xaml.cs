using Callisto.Controls;
using OTWB.Coordinates;
using OTWB.Lattice;
using OTWB.MyControls;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
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
    public sealed partial class LatticeFacePage : OTWB.Common.LayoutAwarePage
    {
        ViewModel viewModel;

        public LatticeFacePage()
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

        private void Export_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ExportCurrentPatternData();
        }

        private void Import_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ImportPattern(PatternType.latticeRim);
            PatternChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private async void ExportPaths_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.ExportCurrentPath();
        }

        private void Gcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GcodePage));
            }
        }

        private void Clear_Lattice_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CurrentPathData is LatticeData)
                (viewModel.CurrentPathData as LatticeData).Clear();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = LatticePathDisplay.Grid;
            gc.Background = LatticePathDisplay.CanvasBackgroundBrush;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            settings.HeaderBrush = new SolidColorBrush(LatticePathDisplay.Grid.Foreground);
            settings.HeaderText = "Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = LatticePathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
           
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (LatticePathDisplay.CurrentPath == null) return;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            //settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
            settings.HeaderText = "Points"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = LatticePathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            PointsControl pc = new PointsControl();
            pc.DataContext = LatticePathDisplay.CurrentPath.AllPoints;
            //pc.SelectedPath = 0;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            viewModel.LatticeDataChanged -= viewModel_LatticeDataChanged;
            SettingsPane.GetForCurrentView().CommandsRequested -= LatticePage_CommandsRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += LatticePage_CommandsRequested;
            try
            {
                base.OnNavigatedTo(e);
                LatticePathDisplay.Grid.Dim1.End = 200;
                viewModel = App.viewModel;
                viewModel.SetupPattern(PatternType.latticeFace, null, -1);
                viewModel.LatticeDataChanged += viewModel_LatticeDataChanged;
                
                this.DataContext = viewModel;
                IncrementCombo.SelectedValue = viewModel.Increment;
                PatternChoices.ItemsSource = viewModel.LatticePatterns;
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

        void LatticePage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("Lattice", "Layout", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
                settings.HeaderText = "Layout Values"; // string.Format("{0}", App.VisualElements.DisplayName);
                settings.ContentBackgroundBrush = LatticePathDisplay.CanvasBackgroundBrush;
                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                LatticeFaceLayoutSettingsContent c = new LatticeFaceLayoutSettingsContent();
                c.DataContext = viewModel.CurrentPathData;
                settings.Content = c;
                // open it
                settings.IsOpen = true;

            });
            args.Request.ApplicationCommands.Add(cmd);
        }

        private void viewModel_LatticeDataChanged(object sender, EventArgs e)
        {
            ReCalculate();  
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
                LatticePathDisplay.Viewmodel = viewModel;
                LatticePathDisplay.CurrentPath = viewModel.CurrentPath;
                LatticePathDisplay.ShowPaths();
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
                LatticeData bc = (LatticeData)PatternChoices.SelectedValue;
                viewModel.CurrentPathData = bc;
                LatticeDisplay.Lattice = bc;
                if (Bigdisplay.IsOpen)
                    PopupDisplay.Lattice = bc;
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

        private void New_Lattice_Click(object sender, RoutedEventArgs e)
        {
            int nxtfree = viewModel.LatticePatterns.Count;
            viewModel.LatticePatterns.Add(new LatticeData(nxtfree + 1));
            PatternChoices.SelectedIndex = nxtfree;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Bigdisplay.IsOpen = true;
            SmallDisplay.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //BigDisplayButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Bigdisplay_Opened(object sender, object e)
        {
            CompositeTransform t = (Bigdisplay.RenderTransform as CompositeTransform);
            t.TranslateX = -(Window.Current.CoreWindow.Bounds.Width * 0.5);
            t.TranslateY = - Window.Current.CoreWindow.Bounds.Height * 0.1;
            PopupDisplay.Lattice = (LatticeData)PatternChoices.SelectedItem;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Bigdisplay.IsOpen = false;
            Line2D l = (Line2D)LatticeLineList.SelectedItem;
            string name = (l == null) ? string.Empty : l.Name;
            LatticeDisplay.Hilight(name, true);
            SmallDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            
        }

        private void LatticeLineList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // here we want to hilight the selected line
            Line2D l = (Line2D)LatticeLineList.SelectedItem;
            string name = (l == null) ? string.Empty : l.Name;
            if (Bigdisplay.IsOpen)
                    PopupDisplay.Hilight(name, true);
                else
                    LatticeDisplay.Hilight(name, true);
        }

        private void Add_Line_Click(object sender, RoutedEventArgs e)
        {
            LatticeData ld = (viewModel.CurrentPathData as LatticeData);
            if (ld == null) return;
            Line2D l = new Line2D(new Point(0, 0), new Point(ld.Columns - 1, ld.Rows - 1));
            ld.Add(l);
        }

        private void Remove_Line_Click(object sender, RoutedEventArgs e)
        {
            LatticeData ld = (viewModel.CurrentPathData as LatticeData);
            if (ld == null) return;
            ld.Remove((Line2D)LatticeLineList.SelectedItem);
        }

        private void LatticePosition_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (viewModel != null)
                if (viewModel.CurrentPathData is LatticeData)
                {
                    (viewModel.CurrentPathData as LatticeData).Layout.ToolPosition = e.NewValue;
                }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Show layout settings
            ShowLayoutSettings();
        }

        private void ShowLayoutSettings()
        {
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
            settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
            settings.HeaderText = "Layout Values"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = LatticePathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;

            // set the content for the flyout
            LatticeFaceLayoutSettingsContent c = new LatticeFaceLayoutSettingsContent();
            c.DataContext = viewModel.CurrentPathData;
            settings.Content = c;
            // open it
            settings.IsOpen = true;           
        }

        private void ToggleWorkButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            LatticePathDisplay.ShowWorkOutline = (bool)tb.IsChecked;
        }

        private void ToggleGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            LatticePathDisplay.Showgrid = (bool)tb.IsChecked;
        }

        private void NumericUpDown_SelectionChanged(object sender, RoutedEventArgs e)
        {
            LatticePathDisplay.PathWidth = PathWidthUpdown.Value;
        }

    }
}
