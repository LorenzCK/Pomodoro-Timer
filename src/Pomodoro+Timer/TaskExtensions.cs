using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace PomodoroTimer {

    public static class TaskExtensions {

        public static void Forget(this IAsyncAction action) {
            // Noop
        }

        public static void Forget(this Task t) {
            // Noop
        }

    }

}
