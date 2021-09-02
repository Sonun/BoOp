using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _password;
        private string _username;

        public DelegateCommand LoginCommand { get; set; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                LoginCommand.OnExecuteChanged();
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                LoginCommand.OnExecuteChanged();
            }
        }

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new DelegateCommand(
                (o) =>
                {
                    return !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password);
                },

                (o) =>
                {
                    Password = "";
                    Username = "";
                }
            );

            Password = "passwort";
            Username = "Benutzername";
        }
    }
}
