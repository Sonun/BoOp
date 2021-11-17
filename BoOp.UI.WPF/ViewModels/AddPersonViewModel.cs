using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: AddPersonViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 4/10/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Viewmodel für die Personen hinzufügen View
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AddPersonViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CreateBarcodeCommand { get; set; }

        //user info attributes
        private string _vorname;
        private string _nachname;
        private string _passwort;
        private string _rechte;
        private int _rechteAsInt;
        private string _telefon;
        private string _email;
        private DateTime _geburtstag;
        private bool _addToPrintList;

        //user Info Propertys
        public bool AddToPrintList
        {
            get
            {
                return _addToPrintList;
            }
            set
            {
                _addToPrintList = value;
                OnPropertyChanged();
            }
        }
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

        public DateTime Geburtstag
        {
            get
            {
                return _geburtstag;
            }
            set
            {
                _geburtstag = value;
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

        public AddPersonViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _library = library;
            _navigationService = navigationService;

            AddToPrintList = true;
            Vorname = "";
            Nachname = "";
            Passwort = "";
            Rechte = "Leser";
            Telefon = "";
            Email = "";
            Geburtstag = new DateTime(2000, 1, 1);

            CancelCommand = new DelegateCommand(
                x => 
                {
                    _navigationService.ShowAdminView(user);
                });

            SaveCommand = new DelegateCommand(
                x =>
                {
                    switch (_rechte.ToLower())
                    {
                        case "leser":
                            _rechteAsInt = 1;
                            break;
                        case "helfer":
                            _rechteAsInt = 2;
                            break;
                        case "bibo team":
                            _rechteAsInt = 4;
                            break;
                        case "admin":
                            _rechteAsInt = 8;
                            break;
                    }

                    if (_vorname.Equals("") || _nachname.Equals("") || (_rechteAsInt > 1 && _passwort.Equals("")))
                    {
                        MessageBox.Show("Bitte fülle alle Nötigen felder aus, bei Benutzern die höher gestellt sind als Leser, muss ein Passwort vergeben werden!");
                        return;
                    }

                    var newUser = new PersonModel
                    {
                        Vorname = _vorname,
                        Nachname = _nachname,
                        PasswortHash = Utils.HashSHA(_passwort),
                        Rechte = (Rechtelevel)_rechteAsInt,
                        GeburtsdatumString = Geburtstag.ToString("d"),
                        Telefonnummer = _telefon,
                        EMail = _email
                    };

                    //add barcode to user
                    do
                    {
                        newUser.AusweisID = Utils.GenerateUniqueUserIDString();
                    } while (!_library.CheckAvailabilityUserID(newUser.AusweisID));
                    
                    _library.AddUser(newUser);

                    if (AddToPrintList)
                    {
                        AdminViewModel.StaticUserIDPrintList.Add(new Common.PersonViewModel(newUser, navigationService, library, user, null));
                    }
                    _navigationService.ShowAdminView(user);
                });
        }
    }
}
