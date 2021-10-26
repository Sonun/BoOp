using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class RemoveBookViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;
        private PersonModel _user;
        private BuchModel _book;

        public BuchModel Book { get { return _book; } set { _book = value; OnPropertyChanged(); } }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public RemoveBookViewModel(INavigationService navigationService, ILibrary library, PersonModel user, BuchModel book)
        {
            _navigationService = navigationService;
            _library = library;
            _user = user;
            Book = book;

            DeleteCommand = new DelegateCommand(
                x =>
                {
                    if (_book != null)
                    {
                        if (MessageBox.Show("Wollen Sie das Buch " + _book.BasicInfos.Titel + " wirklich Löschen? \n Es werden dann auch Alle Exemplare aus der Datenbank gelöscht!", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            _library.RemoveBook(_book);
                            _navigationService.ShowLibraryView(user);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Barcode wurde nicht gefunden");
                    }
                });

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAdminView(user);
                });
        }
    }
}
