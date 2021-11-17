using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Windows;
using System.Timers;
using System.Windows.Threading;
using BoOp.Business;
using System.Diagnostics;
using BoOp.DBAccessor.Models;
using System.Windows.Controls;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ScanUserViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 2/09/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : View model für den buch abgeben view
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ScanUserViewModel : ViewModel
    {
        public static Timer LogoutTimer;

        //time untill logout in minutes
        private readonly int _logoutTimespan = 12;
        private bool _timeFlag;
        private INavigationService _navigationService;
        private ILibrary _library;
        private string _personBarcode;

        public string PersonBarcoded { get { return _personBarcode; } set { _personBarcode = value; OnPropertyChanged(); } }
        public string LogoutWarning { get { return "Sie werden nach \"Weiter\"\nnach " + _logoutTimespan + " Minuten wieder ausgeloggt"; } }
        public TextBox ScanBox { get; set; }

        public DelegateCommand LoginButtonCommand { get; set; }

        public ScanUserViewModel(INavigationService navigationService, ILibrary library, Dispatcher dispatcher)
        {
            ScanBox = new TextBox();
            ScanBox.Focus();
            _navigationService = navigationService;
            _library = library;
            _timeFlag = false;
            if  (LogoutTimer != null) LogoutTimer.Stop();
            LogoutTimer = null;
            AdminViewModel.StaticBookPrintList = new System.Collections.ObjectModel.ObservableCollection<Common.ExemplarViewModel>();
            AdminViewModel.StaticUserIDPrintList = new System.Collections.ObjectModel.ObservableCollection<Common.PersonViewModel>();

            if(_library.GetAllUsers().Count <= 0)
            {
                //standard benutzer
                _library.AddUser(new PersonModel{AusweisID = "RickSanchez", Vorname = "Admin", Nachname = "Admin", Geburtsdatum = new DateTime(2011, 11, 11), Telefonnummer="11", Rechte = Rechtelevel.ADMIN, EMail="11", PasswortHash= "C71FC1B21A6824082BB07D3BF6695D4737D8691B57CEB34E592A8BDE11CB4EA3" });
            }

            LoginButtonCommand = new DelegateCommand
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
