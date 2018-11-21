using System;
using Windows.Storage;

namespace PomodoroTimer {

    public static class Settings {

        private static ApplicationDataContainer Local {
            get => ApplicationData.Current.LocalSettings;
        }

        private const string PomodoroRunLengthKey = "pomodoroLength";

        public static TimeSpan PomodoroRunLength {
            get {
                if (!Local.Values.ContainsKey(PomodoroRunLengthKey))
                    return TimeSpan.FromMinutes(25);
                else
                    return new TimeSpan((long)Local.Values[PomodoroRunLengthKey]);
            }
            set {
                Local.Values[PomodoroRunLengthKey] = value.Ticks;
            }
        }

        private const string RestRunLengthKey = "restLength";

        public static TimeSpan RestRunLength {
            get {
                if (!Local.Values.ContainsKey(RestRunLengthKey))
                    return TimeSpan.FromMinutes(5);
                else
                    return new TimeSpan((long)Local.Values[RestRunLengthKey]);
            }
            set {
                Local.Values[RestRunLengthKey] = value.Ticks;
            }
        }

    }

}
