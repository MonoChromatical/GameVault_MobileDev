using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameVault.ViewModels
{
    // FLOW:
    // XAML pages bind to ViewModel properties.
    // When a property changes, the ViewModel calls OnPropertyChanged.
    // MAUI then refreshes the matching bound controls on screen.
    public class BaseViewModel : INotifyPropertyChanged
    {
        // Backing fields store the real values behind the public properties.
        private bool isBusy;
        private string message = string.Empty;

        // PropertyChanged is the event MAUI listens to for MVVM data binding updates.
        public event PropertyChangedEventHandler? PropertyChanged;

        // IsBusy can be used later to show loading indicators while data loads.
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                // If the value did not change, there is no reason to refresh the UI.
                if (isBusy == value)
                {
                    return;
                }

                isBusy = value;
                OnPropertyChanged();
            }
        }

        // Message can be used later to show simple status or error text on a page.
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message == value)
                {
                    return;
                }

                message = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                // This tells MAUI which property changed so bound XAML can update.
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
