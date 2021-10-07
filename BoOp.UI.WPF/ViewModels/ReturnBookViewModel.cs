using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
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
                    _library.ReturnBook(_barcode);
                    _navigationService.ShowLibraryView(user);
                });

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowLibraryView(user);
                });
        }
    }
}
