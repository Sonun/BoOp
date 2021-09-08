using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using BoOp.Business;
using System;

namespace BoOp.UI.WPF.ViewModels
{
  public class LoginViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _password;
        private string _username;
        private string _userPasswordHash;

        public DelegateCommand LoginCommand { get; set; }

        public string Password
        {
            get
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

        public LoginViewModel(INavigationService navigationService, int userID)
        {
            _navigationService = navigationService;
            _password = "";
            Password = "";

            //this should get the username from the database
            Username = "Benutzer" + userID;

            //this should be the databse password of the user
            _userPasswordHash = Utils.HashSHA("123");

            LoginCommand = new DelegateCommand(
                x =>
                {
                    //compare the password input password with the user password
                    if (Utils.HashSHA(_password).Equals(_userPasswordHash))
                    {
                        //passwort richtig
                        _navigationService.ShowBookView();
                    }
                    else
                    {
                        //falsches passwort
                        _password = "";
                        Password = "";
                    }
                }
            );
        }
    }
}
