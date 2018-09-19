using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PomodoroTimer.ValueConverters {

    class InactiveToBackgroundBrush : IValueConverter {

        private readonly Brush _activeBrush, _inactiveBrush;

        public InactiveToBackgroundBrush() {
            _activeBrush = (Brush)Application.Current.Resources["PomodoroBackgroundBrush"];
            _inactiveBrush = (Brush)Application.Current.Resources["PomodoroDisabledBackgroundBrush"];
        }

        public object Convert(object value, Type targetType, object parameter, string language) {
            bool disabled = (bool)value;
            return disabled ? _inactiveBrush : _activeBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return null;
        }

    }

}
