using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace PomodoroTimer.ViewModels {

    public class TimerViewModel : BaseModel {

        private readonly Timer _timer;
        private readonly CoreDispatcher _dispatcher;

        public TimerViewModel() {
            _timer = new Timer(Timer_Tick, null, Timeout.Infinite, Timeout.Infinite);
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        private void Timer_Tick(object state) {
            _dispatcher.RunAsync(CoreDispatcherPriority.Low, () => {
                var r = _timerTarget - DateTime.Now;
                Remaining = (r.Ticks >= 0) ? r : TimeSpan.Zero;
            }).Forget();
        }

        private bool _isRunning = false;

        public bool IsRunning {
            get => _isRunning;
            set {
                if(SetProperty(ref _isRunning, value)) {
                    if (value) {
                        StartPomodoro();
                    }
                    else {
                        EndPomodoro();
                    }
                }
            }
        }

        DateTime _timerTarget = DateTime.MaxValue;
        TimeSpan _remaining = Settings.PomodoroRunLength;
        TimeSpan _lastLength = Settings.PomodoroRunLength;

        public TimeSpan Remaining {
            get => _remaining;
            private set {
                SetProperty(ref _remaining, value);
                OnPropertyChanged(nameof(Progress));
            }
        }

        public int Progress {
            get {
                return (int)(100.0 * ((_lastLength.Ticks - _remaining.Ticks) / (double)_lastLength.Ticks));
            }
        }

        private void StartPomodoro() {
            Debug.WriteLine("Pomodoro started");

            _lastLength = Settings.PomodoroRunLength;
            _timerTarget = DateTime.Now.Add(_lastLength);
            Remaining = _lastLength;

            Debug.WriteLine("Target {0}, remaining {1}", _timerTarget, Remaining);

            _timer.Change(0, 1000);
        }

        private void EndPomodoro() {
            Debug.WriteLine("Pomodoro stopped");

            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

    }

}
