﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PomodoroTimer.ViewModels {

    public class BaseModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null) {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}