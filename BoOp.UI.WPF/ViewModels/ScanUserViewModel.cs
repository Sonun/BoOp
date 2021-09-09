using BoOp.Business.IO;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BoOp.UI.WPF.ViewModels
{
    public class ScanUserViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _status;
        private Scanner _scanner;
        private Dispatcher _dispatcher;
        private bool _isScanning;

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

        public ScanUserViewModel(INavigationService navigationService, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _dispatcher = dispatcher;
            _scanner = new Scanner();
            _scanner.BarcodeScanned += _scanner_BarcodeScanned;
            Status = "Bitte Scann deine Karte\n   mit dem Kartenleser!";

            TestButtonCommand = new DelegateCommand
                (
                     x =>
                     {
                         _navigationService.ShowLoginView();
                     }
                );

            Scan();
        }

        private void _scanner_BarcodeScanned(Barcode barcode)
        {
            Status = "Barcode gescannt mit ID: \n" + barcode.Text;
            Thread.Sleep(2000);
            _navigationService.ShowLoginView();
            _isScanning = false;
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
