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
        private string _gebTag;
        private string _gebMonat;
        private string _gebJahr;
        private string _telefon;
        private string _email;

        private string _barcode;
        private PersonModel _user;
        private PersonModel _userToChange;

        //user Info Propertys
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

        public string Barcode
        {
            get
            {
                return _barcode;
            }
            set
            {
                _barcode = value;

                // TODO:
                //if (_library.GetUserByBarcode(_barcode).Rechte >= _user.Rechte)
                //{
                //    BarcodeScanned();
                //    OnPropertyChanged();
                //}

                if (_library.GetUserByID(2).Rechte <= _user.Rechte)
                {
                    BarcodeScanned();
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Du darfst diesen Benutzer nicht editieren, oder er existiert nicht");
                }
            }
        }

        public EditPersonViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            _library = library;
            _navigationService = navigationService;
            _user = user;

            Vorname = "";
            Nachname = "";
            Passwort = "";
            Rechte = "";
            GebTag = "";
            GebMonat = "";
            GebJahr = "";
            Telefon = "";
            Email = "";

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAdminView(_user);
                });

            SaveCommand = new DelegateCommand(
                x =>
                {
                    //biboteam darf keinen admin hinzufügen
                    if (_rechteAsInt == 8 && _user.Rechte <= Rechtelevel.BIBOTEAM)
                    {
                        MessageBox.Show("du darfst keinen Admin erstellen");
                        return;
                    }

                    switch (_rechte.ToLower())
                    {
                        case "leser":
                            _rechteAsInt = 1;
                            break;
                        case "helfer":
                            _rechteAsInt = 2;
                            break;
                        case "biboteam":
                            _rechteAsInt = 4;
                            break;
                        case "admin":
                            _rechteAsInt = 8;
                            break;
                    }

                    if (_vorname.Equals("") || _nachname.Equals("") || _gebTag.Equals("") || _gebMonat.Equals("") || _gebJahr.Equals("") || _telefon.Equals(""))
                    {
                        MessageBox.Show("Sie müssen alles (bis auf das Passwort) eingeben");
                        return;
                    }

                    var tempModel = new PersonModel
                    {
                        Id = _userToChange.Id,
                        AusweisID = _userToChange.AusweisID,
                        Vorname = _vorname,
                        Nachname = _nachname,
                        Rechte = (Rechtelevel)_rechteAsInt,
                        Geburtsdatum = new DateTime(int.Parse(_gebJahr), int.Parse(_gebMonat), int.Parse(_gebTag)),
                        Telefonnummer = _telefon,
                        EMail = _email
                    };

                    if (_user.Rechte != Rechtelevel.ADMIN && !_passwort.Equals(""))
                    {
                        MessageBox.Show("Nur Admins können passwörter ändern, passwort wurde nicht geändert");
                        Passwort = "";
                    }

                    tempModel.PasswortHash = _passwort != "" ? Utils.HashSHA(_passwort) : _userToChange.PasswortHash;

                    _library.EditUserDetails(tempModel);
                    _navigationService.ShowAdminView(_user);
                });
        }

        private void BarcodeScanned()
        {
            //TODO:
            //_userToChange = _library.GetUserByBarcode(_barcode);
            _userToChange = _library.GetUserByID(2);

            Nachname = _userToChange.Nachname;
            Vorname = _userToChange.Vorname;
            Email = _userToChange.EMail;
            Telefon = _userToChange.Telefonnummer;
            GebJahr = _userToChange.Geburtsdatum.Year.ToString();
            GebMonat = _userToChange.Geburtsdatum.Month.ToString();
            GebTag = _userToChange.Geburtsdatum.Day.ToString();
            Rechte = _userToChange.Rechte.ToString();
        }
    
    }
}
