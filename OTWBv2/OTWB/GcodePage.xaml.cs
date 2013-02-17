using Geometric_Chuck.MyControls;
using Callisto.Controls;
using OTWB.Coordinates;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Shapes;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Imaging;
using OTWB.Settings;
using System.Diagnostics;
using Windows.Storage.Pickers;
using OTWB.CodeGeneration;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Geometric_Chuck
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GcodePage : Geometric_Chuck.Common.LayoutAwarePage
    {
        ViewModel viewModel;
        CodeGenViewModel codeGen;

        public enum ListType
        {
            POINT,
            CYLINDRICAL,
            POINT_OFFSET,
            CYLINDRICAL_OFFSET
        }

        public GcodePage()
        {
            this.InitializeComponent();
            codeGen = new CodeGenViewModel();
            CodeViewer.DataContext = codeGen.Code;
            DataContext = codeGen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel = (ViewModel)e.Parameter;
            SettingsPane.GetForCurrentView().CommandsRequested += GcodePage_CommandsRequested;
            
            if ((viewModel.CurrentPath == null) || (viewModel.CurrentPath.Count == 0))
                codeGen.ImportPath();
            else
            {
                codeGen.CurrentPath = viewModel.CurrentPathAsListofPoint;
                codeGen.PathName = viewModel.CurrentPath.PatternName;
            }
            codeGen.GenerateCode();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= GcodePage_CommandsRequested;
            base.OnNavigatedFrom(e);
            
        }

        private void GcodePage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("GCode", "Code Generation", (x) =>
            {
                // create a new instance of the flyout
                SettingsFlyout settings = new SettingsFlyout();
                // set the desired width.  If you leave this out, you will get Narrow (346px)
                settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;

                // optionally change header and content background colors away from defaults (recommended)
                // if using Callisto's AppManifestHelper you can grab the element from some member var you held it in
                // settings.HeaderBrush = new SolidColorBrush(App.VisualElements.BackgroundColor);
                settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
                settings.HeaderText = "G-Code"; // string.Format("{0}", App.VisualElements.DisplayName);

                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                CodeSettingsContent c = new CodeSettingsContent();
                c.CodeTemplates = codeGen.Templates;
                settings.Content = c;
                // open it
                settings.IsOpen = true;

            });

            args.Request.ApplicationCommands.Add(cmd);
        }
        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            SettingsPane.Show();
        }

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

        private PointsControl SetupPointsControl(ListType typ)
        {
            PointsControl pc = new PointsControl();
            switch (typ)
            {
                case ListType.POINT_OFFSET:
                    pc.DataContext = codeGen.CreateOffsetList;
                    pc.Title = "Offsets";
                    break;
                case ListType.POINT:
                    pc.DataContext = codeGen.CurrentPath;
                    pc.Title = "Points";
                    break;
                case ListType.CYLINDRICAL:
                    pc.DataContext = codeGen.CreateCylindricalList;
                    pc.Title = "Cylindrical Points";
                    break;
                case ListType.CYLINDRICAL_OFFSET:
                    pc.DataContext = codeGen.CreateCylindricalOffsetList;
                    pc.Title = "Cylindrical Offsets";
                    break;
            }
            pc.SelectedPath = 0;

            return pc;
        }
        
        private void Points_Click(object sender, RoutedEventArgs e)
        {

            if (codeGen.CurrentPath.Count == 0) return;
            string param = (string)(sender as Button).CommandParameter;
            ListType flag = (ListType)Enum.Parse(typeof(ListType), param);
            PointsControl pc = SetupPointsControl(flag);
            TCD.Controls.Flyout f = new TCD.Controls.Flyout(
             new SolidColorBrush(Colors.White),//the foreground color of all flyouts
             (Brush)App.Current.Resources["ApplicationPageBackgroundThemeBrush"],//the background color of all flyouts
             new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),//the theme brush of the app
             pc.Title,
             FlyoutDimension.Narrow,//switch between narrow and wide depending on the check box
             pc);
            f.ShowAsync();
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

        private void Generate_Code_Click(object sender, RoutedEventArgs e)
        {
            codeGen.GenerateCode();
           
        }

        private void Clear_Code_Click(object sender, RoutedEventArgs e)
        {
            codeGen.Clear();

        }

        private void Edit_Templates_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Templates_Click(object sender, RoutedEventArgs e)
        {
            codeGen.SaveTemplateCollection();
        }

        private void Load_Templates_Click(object sender, RoutedEventArgs e)
        {
            codeGen.LoadTemplates();
        }
    }
}
