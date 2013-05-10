using Callisto.Controls;
using OTWB.Braid;
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
    public sealed partial class BraidPage : OTWB.Common.LayoutAwarePage
    {
        ViewModel viewModel;

        public BraidPage()
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

        private async void ExportPaths_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.ExportCurrentPath();
        }

        private void Export_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ExportCurrentPatternData();
        }

        private void Import_Pattern_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ImportPattern(PatternType.braid);
            PatternChoices.SelectedIndex = viewModel.SelectedPathIndex;
        }

        private void Clear_Braid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Gcode_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(GcodePage));
            }
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            GridControl gc = new GridControl();
            gc.DataContext = BraidPathDisplay.Grid;

            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(BraidPathDisplay.Grid.Foreground);
            settings.HeaderText = "Change Grid Size"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = BraidPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = gc;
            // open it
            settings.IsOpen = true;
        }

        private void Points_Click(object sender, RoutedEventArgs e)
        {
            if (BraidPathDisplay.CurrentPath.Count == 0) return;
            PointsControl pc = new PointsControl();
            pc.DataContext = viewModel.CurrentPath.AllPaths; //BazleyPathDisplay.CurrentPath.AllPoints;
            pc.SelectedPath = 0;
            SettingsFlyout settings = new SettingsFlyout();
            // set the desired width.  If you leave this out, you will get Narrow (346px)
            settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;

            // optionally change header and content background colors away from defaults (recommended)
            // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
            settings.HeaderBrush = new SolidColorBrush(BraidPathDisplay.Grid.Foreground);
            settings.HeaderText = "Points"; // string.Format("{0}", App.VisualElements.DisplayName);
            settings.ContentBackgroundBrush = BraidPathDisplay.CanvasBackgroundBrush;
            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;
            settings.Content = pc;
            // open it
            settings.IsOpen = true;

        }

        private void Add_Strand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remove_Strand_click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            viewModel.BraidDataChanged -= viewModel_BraidDataChanged;
            //viewModel.CleanUpForPageChange();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);
                //viewModel = (ViewModel)e.Parameter;
                viewModel = App.viewModel;
                viewModel.SetupPattern(PatternType.braid, null, -1);
                viewModel.BraidDataChanged += viewModel_BraidDataChanged;

                this.DataContext = viewModel;
                IncrementCombo.SelectedValue = viewModel.Increment;
                PatternChoices.ItemsSource = viewModel.BraidPatterns;
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

        private void viewModel_BraidDataChanged(object sender, EventArgs e)
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
                BraidPathDisplay.CurrentPath = viewModel.CurrentPath;
                BraidPathDisplay.ShowPaths();
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
                BraidData bc = (BraidData)PatternChoices.SelectedValue;
                viewModel.CurrentPathData = bc;
                BraidDisplay.Braid = bc;
                if (Bigdisplay.IsOpen)
                    PopupDisplay.Braid = bc;
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

        private void Add_Perm_Click(object sender, RoutedEventArgs e)
        {
             BraidData bc = viewModel.CurrentPathData as BraidData;
             Permutation p = Permutation.CreateID(bc.NumStrands);
             p.Index = bc.Perms.Count;
             bc.Perms.Add(p);
             ReCalculate();
             BraidDisplay.Braid = bc;
        }

        private void Remove_Perm_Click(object sender, RoutedEventArgs e)
        {
            BraidData bc = viewModel.CurrentPathData as BraidData;
            bc.RemoveLastPerm();
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
            t.TranslateY = -Window.Current.CoreWindow.Bounds.Height * 0.1;
            PopupDisplay.Braid = (BraidData)PatternChoices.SelectedItem;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Bigdisplay.IsOpen = false;
            SmallDisplay.Visibility = Windows.UI.Xaml.Visibility.Visible;
            BraidDisplay.Show();
        }

        private void BraidPermList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // change color of perm to hilight ??
            BraidDisplay.Hilight(BraidPermList.SelectedIndex);
        }

        private void BraidPosition_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (viewModel != null)
                if (viewModel.CurrentPathData is BraidData)
                {
                    (viewModel.CurrentPathData as BraidData).ToolPosition = e.NewValue;
                    ReCalculate();
                }
        }

       
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            int indx = c.SelectedIndex;
            
        }

        private void Show_Layout_Click(object sender, RoutedEventArgs e)
        {
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

            // provide some logo (preferrably the smallogo the app uses)
            BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
            settings.SmallLogoImageSource = bmp;

            // set the content for the flyout
            BraidLayoutSettingsContent b = new BraidLayoutSettingsContent();
            b.DataContext = viewModel.CurrentPathData;
            settings.Content = b;
            // open it
            settings.IsOpen = true;
        }
    }
}
