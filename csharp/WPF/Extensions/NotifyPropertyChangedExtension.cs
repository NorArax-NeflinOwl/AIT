using System.Collections.Generic;
using System.ComponentModel;

namespace WPF.ExtendedClasses
{
    public class NotifyPropertyChangedExtension : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyPropertyChangedExtension(PropertyChangedEventHandler propertyChanged)
        {
            PropertyChanged = propertyChanged;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
