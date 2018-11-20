using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace PomodoroTimer.ViewModels {

    public class ApplicationViewModel : BaseModel {

        public ApplicationViewModel() {
            Window.Current.Activated += HandleWindowActivated;

            var coreWindow = CoreApplication.GetCurrentView();
            if (coreWindow != null) {
                coreWindow.TitleBar.LayoutMetricsChanged += HandleTitleBarLayoutMetricsChanged;
            }
        }

        private void HandleWindowActivated(object sender, WindowActivatedEventArgs e) {
            IsInactive = e.WindowActivationState == CoreWindowActivationState.Deactivated;
        }

        private bool _isInactive = false;

        public bool IsInactive {
            get => _isInactive;
            set {
                SetProperty(ref _isInactive, value);
            }
        }

        private void HandleTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
            var coreWindow = CoreApplication.GetCurrentView();
            if(coreWindow != null) {
                if(coreWindow.TitleBar != null && coreWindow.TitleBar.Height > 0) {
                    TitleBarHeight = coreWindow.TitleBar.Height;
                }
            }
        }

        private double _titleBarHeight = 32.0;

        public double TitleBarHeight {
            get => _titleBarHeight;
            set {
                SetProperty(ref _titleBarHeight, value);
            }
        }

    }

}
