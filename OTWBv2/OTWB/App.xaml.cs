using Callisto.Controls;
using Callisto.Controls.SettingsManagement;
using OTWB.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Geometric_Chuck
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private ViewModel viewModel { get; set; }
        public static Callisto.Controls.Common.VisualElement VisualElements;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            viewModel = new ViewModel();
           
            this.Suspending += OnSuspending;
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand(1, "Help", HelpCommand));
        }

        private async void HelpCommand(Windows.UI.Popups.IUICommand command)
        {
            try
            {
                Windows.Storage.StorageFile file =
                    await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Assets\OTWBHelp.pdf");
                await Windows.System.Launcher.LaunchFileAsync(file);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            AppSettings.Current.AddCommand<CodeSettingsContent>("App Registered", SettingsFlyout.SettingsFlyoutWidth.Wide);

            VisualElements = await Callisto.Controls.Common.AppManifestHelper.GetManifestVisualElementsAsync();
            Frame rootFrame = Window.Current.Content as Frame;

            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                Geometric_Chuck.Common.SuspensionManager.RegisterFrame(rootFrame, "appFrame");
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                    await Geometric_Chuck.Common.SuspensionManager.RestoreAsync();
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(StartPage), viewModel)) // args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            await Geometric_Chuck.Common.SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
