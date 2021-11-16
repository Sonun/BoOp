using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using BoOp.Business;
using System.Windows;
using BoOp.DBAccessor.Models;

namespace BoOp.UI.WPF.ViewModels
{
  public class LoginViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _password;
        private string _username;
        private string _userPasswordHash; 
        private ILibrary _library;

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public string Password
        {
            private get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _navigationService = navigationService;
            if (user.Rechte < Rechtelevel.BIBOTEAM)
                _navigationService.ShowLibraryView(user);

            _library = library;
            _password = "";
            Password = "";

            //this should get the username from the database
            Username = user.Vorname + " " + user.Nachname;

            //this should be the databse password of the user
            _userPasswordHash = user.PasswortHash;

            LoginCommand = new DelegateCommand(
                x =>
                {
                    //compare the password input password with the user password
                    if (Utils.HashSHA(_password).Equals(_userPasswordHash))
                    {
                        //passwort richtig
                        _navigationService.ShowAdminView(user);
                    }
                    else
                    {
                        //falsches passwort
                        MessageBox.Show("Passwort war falsch");
                        Password = "";
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
