using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Windows;
using System.Timers;
using System.Windows.Threading;
using BoOp.Business;
using System.Diagnostics;

namespace BoOp.UI.WPF.ViewModels
{
    public class ScanUserViewModel : ViewModel
    {
        public static Timer LogoutTimer;

        //time untill logout in minutes
        private readonly int _logoutTimespan = 1;
        private bool _timeFlag;
        private INavigationService _navigationService;
        private ILibrary _library;
        private string _personBarcode;

        public string PersonBarcoded { get { return _personBarcode; } set { _personBarcode = value; OnPropertyChanged(); } }
        public string LogoutWarning { get { return "Sie werden nach \"Weiter\"\nnach " + _logoutTimespan + " Minuten wieder ausgeloggt"; } }

        public DelegateCommand TestButtonCommand { get; set; }

        public ScanUserViewModel(INavigationService navigationService, ILibrary library, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _library = library;
            _timeFlag = false;
            if  (LogoutTimer != null) LogoutTimer.Stop();
            LogoutTimer = null;

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
            LogoutTimer = new Timer();
            LogoutTimer.Elapsed += new ElapsedEventHandler(Callback);
            LogoutTimer.Interval = _logoutTimespan * 30000;
            LogoutTimer.Start();
            Debug.WriteLine("user wurde um " + DateTime.Now + " eingeloggt");
        }

        private void Callback(object source, ElapsedEventArgs e)
        {
            if (_timeFlag)
            {
                Debug.WriteLine(DateTime.Now);
                LogoutTimer.Stop();
                LogoutTimer = null;
                _navigationService.ShowScanUserView();
                MessageBox.Show("Du wurdest aus sicherheitsgründen, nach " + _logoutTimespan + " minuten eingeloggt sein, ausgeloggt");
            }
            else
            {
                _timeFlag = true;
            }
        }
    }
}
