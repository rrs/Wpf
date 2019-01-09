using System;
using System.ComponentModel;

namespace Rrs.Wpf.Core
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginInvoke(Action a)
        {
            Invoker.BeginInvoke(a);
        }
    }
}
