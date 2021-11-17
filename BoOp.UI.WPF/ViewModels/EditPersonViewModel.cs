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
    //Dateiname: EditPersonViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 6/10/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Viewmodel für die personen edit View
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class EditPersonViewModel : ViewModel
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
        private string _telefon;
        private string _email;
        private DateTime _birthdate;

        private PersonModel _user;
        private PersonModel _userToChange;

        //user Info Propertys
        public DateTime Birthdate
        {
            get
            {
                return _birthdate;
            }
            set
            {
                _birthdate = value;
                OnPropertyChanged();
            }
        }

        public string Vorname
        {
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

        public EditPersonViewModel(INavigationService navigationService, ILibrary library, PersonModel editor, PersonModel userToChange)
        {
            _library = library;
            _navigationService = navigationService;
            _user = editor;
            _userToChange = userToChange;

            Rechte = userToChange.Rechte.ToString();

            GetUserInfo(_userToChange);

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAdminView(_user);
                });

            SaveCommand = new DelegateCommand(
                x =>
                {
                    if ((Rechtelevel)_rechteAsInt > _user.Rechte)
                    {
                        MessageBox.Show("du darfst keine höher gestellte Person erstellen");
                        return;
                    }

                    if (_vorname.Equals("") || _nachname.Equals("") || _telefon.Equals("") || (userToChange.Rechte == Rechtelevel.LESER && _rechteAsInt >= 2 && _passwort.Equals("")))
                    {
                        MessageBox.Show("Sie müssen alle nötigen Felder eingeben,\nwenn ein Leser höhere rechte bekommt, muss ein passwort vergeben werden.");
                        return;
                    }

                    var tempModel = new PersonModel
                    {
                        Id = _userToChange.Id,
                        AusweisID = _userToChange.AusweisID,
                        Vorname = _vorname,
                        Nachname = _nachname,
                        Rechte = (Rechtelevel)_rechteAsInt,
                        Geburtsdatum = Birthdate,
                        Telefonnummer = _telefon,
                        EMail = _email
                    };

                    if (_user.Rechte != Rechtelevel.ADMIN && !_passwort.Equals(""))
                    {
                        MessageBox.Show("Nur Admins können passwörter ändern, passwort wurde nicht geändert");
                        Passwort = "";
                    }

                    //if password box was empty, dont change
                    tempModel.PasswortHash = _passwort != "" ? Utils.HashSHA(_passwort) : _userToChange.PasswortHash;

                    _library.EditUserDetails(tempModel);
                    _navigationService.ShowAdminView(_user);
                });
        }

        private void GetUserInfo(PersonModel userToChange)
        {
            Nachname = userToChange.Nachname;
            Vorname = userToChange.Vorname;
            Email = userToChange.EMail;
            Passwort = "";
            Telefon = userToChange.Telefonnummer;
            Birthdate = userToChange.Geburtsdatum;


            //set rechte
            switch (userToChange.Rechte.ToString().ToLower())
            {
                case "leser":
                    Rechte = "Leser";
                    break;
                case "helfer":
                    Rechte = "Helfer";
                    break;
                case "biboteam":
                    Rechte = "Bibo Team";
                    break;
                case "admin":
                    Rechte = "Admin";
                    break;
            }
        }
    
    }
}
