using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.ViewModels
{
    public class AddPersonViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        //user info attributes
        private string _vorname;
        private string _nachname;
        private string _passwort;
        private string _rechte;
        private int _rechteAsInt;
        private string _gebTag;
        private string _gebMonat;
        private string _gebJahr;
        private string _telefon;
        private string _email;

        //user Info Propertys
        public string Vorname {
            get 
            {
                return _vorname;
            }
            set
            {
                _vorname = value;
                OnPropertyChanged();
            }
        }

        public string Nachname
        {
            get
            {
                return _nachname;
            }
            set
            {
                _nachname = value;
                OnPropertyChanged();
            }
        }

        public string Passwort
        {
            get
            {
                return _passwort;
            }
            set
            {
                _passwort = value;
                OnPropertyChanged();
            }
        }

        public string Rechte
        {
            get
            {
                return _rechte;
            }
            set
            {
                _rechte = value;
                OnPropertyChanged();
            }
        }

        public string GebTag
        {
            get
            {
                return _gebTag;
            }
            set
            {
                _gebTag = value;
                OnPropertyChanged();
            }
        }

        public string GebMonat
        {
            get
            {
                return _gebMonat;
            }
            set
            {
                _gebMonat = value;
                OnPropertyChanged();
            }
        }

        public string GebJahr
        {
            get
            {
                return _gebJahr;
            }
            set
            {
                _gebJahr = value;
                OnPropertyChanged();
            }
        }

        public string Telefon
        {
            get
            {
                return _telefon;
            }
            set
            {
                _telefon = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public AddPersonViewModel(INavigationService navigationService, ILibrary library)
        {
            _library = library;
            _navigationService = navigationService;
            Rechte = "Leser";

            CancelCommand = new DelegateCommand(
                x => 
                {
                    _navigationService.ShowAdminView();
                });

            SaveCommand = new DelegateCommand(
                x =>
                {
                    _rechteAsInt = 1;
                    switch (_rechte.ToLower())
                    {
                        case "leser":
                            _rechteAsInt = 1;
                            break;
                        case "helfer":
                            _rechteAsInt = 2;
                            break;
                        case "biboteam":
                            _rechteAsInt = 3;
                            break;
                        case "admin":
                            _rechteAsInt = 4;
                            break;
                    }

                    var newUser = new PersonModel
                    {
                        Vorname = _vorname,
                        Nachname = _nachname,
                        PasswortHash = Utils.HashSHA(_passwort),
                        Rechte = (Rechtelevel)_rechteAsInt,
                        Geburtsdatum = _gebTag + "-" + _gebMonat + "-" + _gebJahr,
                        Telefonnummer = _telefon,
                        EMail = _email
                    };

                    _library.AddUser(newUser);

                    _navigationService.ShowAdminView();
                });
        }
    }
}
