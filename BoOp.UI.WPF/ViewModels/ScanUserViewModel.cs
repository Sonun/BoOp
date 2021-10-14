using BoOp.Business.IO;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using BoOp.Business;

namespace BoOp.UI.WPF.ViewModels
{
    public class ScanUserViewModel : ViewModel
    {
        private readonly int _logoutTimespan = 12;
        private Timer logoutTimer;
        private bool timeFlag;

        private INavigationService _navigationService;
        private ILibrary _library;

        private string _personBarcode;

        public string PersonBarcoded { get { return _personBarcode; } set { _personBarcode = value; OnPropertyChanged(); } }
        public string LogoutWarning { get { return "Sie werden nach \"Weiter\", nach " + _logoutTimespan + " Minuten wieder ausgeloggt"; } }

        public DelegateCommand TestButtonCommand { get; set; }

        public ScanUserViewModel(INavigationService navigationService, ILibrary library, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _library = library;
            timeFlag = false;

            TestButtonCommand = new DelegateCommand
                (
                     x =>
                     {
                         try
                         {
                             _navigationService.ShowLibraryView(_library.GetUserByBarcode(_personBarcode));
                             StartLogoutTimer();
                         }
                         catch (Exception e)
                         {
                             MessageBox.Show(e.Message);
                         }
                     }
                );
        }

        private void StartLogoutTimer()
        {
            logoutTimer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromMinutes(_logoutTimespan));
        }

        private void Callback(object state)
        {
            if (timeFlag)
            {
                _navigationService.ShowScanUserView();
                MessageBox.Show("Du wurdest aus sicherheitsgründen, nach " + _logoutTimespan + " minuten eingeloggt sein, ausgeloggt");
            }
            else
            {
                timeFlag = true;
            }
        }
    }
}
