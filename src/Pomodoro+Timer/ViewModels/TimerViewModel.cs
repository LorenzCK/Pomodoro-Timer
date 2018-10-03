using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.ViewModels {

    public class TimerViewModel : BaseModel {

        private bool _isRunning = false;

        public bool IsRunning {
            get => _isRunning;
            set {
                SetProperty(ref _isRunning, value);
                System.Diagnostics.Debug.WriteLine("Running: " + _isRunning);
            }
        }

        public void ToggleTimer() {
            //IsRunning = !IsRunning;
        }

    }

}
