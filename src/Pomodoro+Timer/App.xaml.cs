﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PomodoroTimer {

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application {

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App() {
            InitializeComponent();
            Suspending += OnSuspending;

            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.PreferredLaunchViewSize = new Size(300, 340);

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3)) {
                FocusVisualKind = FocusVisualKind.Reveal;
            }

            DatabaseAccess.Initialize();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user. Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e) {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            SetupAppWindow();

            if (e.PrelaunchActivated == false) {
                if (rootFrame.Content == null) {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Sets up app view window details
        /// </summary>
        private void SetupAppWindow() {
            var appView = ApplicationView.GetForCurrentView();
            appView.SetPreferredMinSize(new Size(230, 250));

            var pomodoroBackColor = (Windows.UI.Color)Current.Resources["PomodoroBackgroundColor"];
            var pomodoroHoverBackColor = (Windows.UI.Color)Current.Resources["PomodoroHoverBackgroundColor"];
            var pomodoroDisabledBackColor = (Windows.UI.Color)Current.Resources["PomodoroDisabledBackgroundColor"];

            var pomodoroForeColor = (Windows.UI.Color)Current.Resources["PomodoroForegroundColor"];

            var titleBar = appView.TitleBar;
            titleBar.BackgroundColor = pomodoroBackColor;
            titleBar.ForegroundColor = pomodoroForeColor;
            titleBar.InactiveBackgroundColor = pomodoroDisabledBackColor;
            titleBar.InactiveForegroundColor = pomodoroForeColor;
            titleBar.ButtonBackgroundColor = pomodoroBackColor;
            titleBar.ButtonForegroundColor = pomodoroForeColor;
            titleBar.ButtonHoverBackgroundColor = pomodoroHoverBackColor;
            titleBar.ButtonInactiveBackgroundColor = pomodoroDisabledBackColor;

            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e) {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            var deferral = e.SuspendingOperation.GetDeferral();

            // Noop

            deferral.Complete();
        }

    }

}
