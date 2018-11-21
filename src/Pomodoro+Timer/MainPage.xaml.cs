using PomodoroTimer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace PomodoroTimer {

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        public TimerViewModel Model { get; private set; }

        public MainPage() {
            this.InitializeComponent();
            TimerProgressBar.Value = 75;

            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;

            DataContext = Model = new TimerViewModel();
        }

        private void TitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender, object args) {
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            System.Diagnostics.Debug.WriteLine("Layout metrics, title bar height: " + coreTitleBar.Height);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            System.Diagnostics.Debug.WriteLine("On navigated to, title bar height: " + coreTitleBar.Height);
        }

        private async void ButtonToggle_Click(object sender, RoutedEventArgs e) {
            var sb = (Storyboard)ButtonToggle.Resources[(ButtonToggle.IsChecked.GetValueOrDefault(false)) ? "ActivateStoryboard" : "DectivateStoryboard"];
            sb.Begin();

            long id = await DatabaseAccess.NewRun();

            await DatabaseAccess.CompleteRun(id);

            Debug.WriteLine("Completed run " + id);
        }
    }

}
