using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PomodoroTimer.ValueConverters {

    public class RemainingTimeConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, string language) {
            var ts = (TimeSpan)value;

            if(ts.Hours > 0) {
                return ts.ToString(@"hh\:mm\:ss");
            }
            else {
                return ts.ToString(@"mm\:ss");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }

    }

}
