using System;
using System.Windows.Input;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: DelegateCommand.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 1/09/2021
    //Bearbeitet von : Dominik von Michalkowsky
    //Beschreibung : DelegateCommand zum aufrufen von methoden über die ui
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void OnExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute?.Invoke(parameter);
    }
}
