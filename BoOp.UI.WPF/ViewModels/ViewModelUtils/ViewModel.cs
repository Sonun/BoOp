using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 1/09/2021
    //Bearbeitet von : Dominik von Michalkowsky
    //Beschreibung : ViewModel klasse die in alle Viewmodels vererbt werden muss
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetValue<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                return false;
            }

            backingField = newValue;

            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if(!string.IsNullOrEmpty(propertyName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
