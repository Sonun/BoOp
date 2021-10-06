using BoOp.Business.IO;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BoOp.Business;

namespace BoOp.UI.WPF.ViewModels
{
    public class ScanUserViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _status;
        private Scanner _scanner;
        private Dispatcher _dispatcher;
        private bool _isScanning;
        private ILibrary _library;

        public DelegateCommand TestButtonCommand { get; set; }

        public string Status {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ScanUserViewModel(INavigationService navigationService, ILibrary library, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _dispatcher = dispatcher;
//            _scanner = new Scanner();
            _library = library;
//            _scanner.BarcodeScanned += _scanner_BarcodeScanned;
            Status = "Bitte Scann deine Karte\n   mit dem Kartenleser!";

            TestButtonCommand = new DelegateCommand
                (
                     x =>
                     {
                         _navigationService.ShowLibraryView(new DBAccessor.Models.PersonModel { Rechte = DBAccessor.Models.Rechtelevel.ADMIN, AusweisID="1", PasswortHash = Utils.HashSHA("123"), Vorname ="Test", Nachname="User", Id=1, EMail="sad@sad.sad", Telefonnummer="123", Geburtsdatum=new DateTime(1996, 07, 20) });
                     }
                );

//            Scan();
        }

        private void _scanner_BarcodeScanned(Barcode barcode)
        {
            /*
            Status = "Barcode gescannt mit ID: \n" + barcode.Text;
            Thread.Sleep(2000);
            _navigationService.ShowLibraryView();
            _isScanning = false;
            */
        }

        public void Scan()
        {
            _isScanning = true;
            _dispatcher.Invoke(() =>
            {
                Task.Run(() =>
                {
                    while (_isScanning)
                    {
                        _scanner.Scan();
                    }
                });
            });
        }

        
    }
}
