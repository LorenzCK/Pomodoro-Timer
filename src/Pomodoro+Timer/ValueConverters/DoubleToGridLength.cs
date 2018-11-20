using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PomodoroTimer.ValueConverters {

    class DoubleToGridLength : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, string language) {
            return new GridLength((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }

    }

}
