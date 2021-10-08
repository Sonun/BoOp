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
        private string _barcode;

        public string Barcode { get { return _barcode; } set { _barcode = value; OnPropertyChanged(); OnPropertyChanged(nameof(SaveCommand)); } }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public RemoveBookViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _navigationService = navigationService;
            _library = library;
            _user = user;
            _barcode = "";

            SaveCommand = new DelegateCommand(
                x =>
                {
                    var bookname = GetBookname(_barcode);
                    if (bookname != null)
                    {
                        if (MessageBox.Show("Wollen Sie das Buch " + bookname + " wirklich Löschen?", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            _library.RemoveBook(_barcode);
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
                    _navigationService.ShowLibraryView(user);
                });
        }

        private string GetBookname(string barcode)
        {
            try {
                var bookId = _library.GetBookIdByBarcode(_barcode);
                return _library.GetAllBooks().SingleOrDefault(x => { return x.Exemplare.ToList().SingleOrDefault(y => { return y.BasicInfos.Barcode == barcode; }).BasicInfos.BuchID == x.BasicInfos.Id; }).BasicInfos.Titel;
            }
            catch
            {
                return null;
            }
        }
    }
}
