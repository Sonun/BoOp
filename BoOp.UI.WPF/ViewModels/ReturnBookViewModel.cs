using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ReturnBookViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 7/10/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : View model für den buch abgeben view
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ReturnBookViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;
        private PersonModel _user;
        private string _barcode;

        public string Username { get { return _user.Vorname; } }
        public string Barcode { get { return _barcode; } set { _barcode = value; OnPropertyChanged(); OnPropertyChanged(nameof(SaveCommand)); } }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public ReturnBookViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _navigationService = navigationService;
            _library = library;
            _user = user;
            _barcode = "";

            SaveCommand = new DelegateCommand(
                x =>
                {
                    try
                    {
                        if (_library.GetBookIdByBarcode(_barcode) == null) throw new System.Exception();
                        _library.ReturnBook(_barcode);
                        _navigationService.ShowLibraryView(user);
                    }
                    catch
                    {
                        MessageBox.Show("Der Barcode konnte keinem Buch zugeordnet werden!");
                    }
                });

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowLibraryView(user);
                });
        }
    }
}
