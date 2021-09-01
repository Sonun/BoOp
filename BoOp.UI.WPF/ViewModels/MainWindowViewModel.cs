using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private string _bookname;
        private string _username;

        public DelegateCommand ClearCommand { get; set; }

        public string Bookname { 
            get => _bookname; 
            set
            {
                _bookname = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UserOwnsBook));
                ClearCommand.OnExecuteChanged();
            }
        }
        public string Username {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UserOwnsBook));
                ClearCommand.OnExecuteChanged();
            }
        }

        public string UserOwnsBook => $"{Username} gehört {Bookname}";

        public MainWindowViewModel()
        {
            ClearCommand = new DelegateCommand(
                (o) =>
                {
                    return !string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Bookname);
                },

                (o) =>
                {
                    Bookname = "";
                    Username = "";
                }
            );

            Bookname = "book";
            Username = "user";
        }
    }
}
