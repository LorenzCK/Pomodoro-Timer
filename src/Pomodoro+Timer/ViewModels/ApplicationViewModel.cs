using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace PomodoroTimer.ViewModels {

    public class ApplicationViewModel : BaseModel {

        public ApplicationViewModel() {
            Window.Current.Activated += HandleWindowActivated;
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

    }

}
