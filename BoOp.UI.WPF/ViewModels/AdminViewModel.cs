using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BoOp.UI.WPF.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;
        private Dispatcher _dispatcher;
        private ObservableCollection<BookViewModel> _bookList;
        private ObservableCollection<BuchModel> _currentList, _originalList;
        private ObservableCollection<PersonViewModel> _userList;
        private ObservableCollection<PersonModel> _currentUserList, _originalUserList;
        private ObservableCollection<ExemplarViewModel> _lendedBookList;
        private PersonModel _user;
        private bool _titleFlag, _authorFlag, _isbnFlag, _vornameFlag, _rechteFlag, _nachnameFlag, _lendeddateflag, _lendednameflag;
        private string _allBookSearchWord, _userSearchWord, _lendedBooksSearchWord;

        public PersonModel LoggedinUser { get { return _user; } }

        public static ObservableCollection<ExemplarViewModel> StaticBookPrintList;
        public static ObservableCollection<PersonViewModel> StaticUserIDPrintList;

        public ObservableCollection<ExemplarViewModel> BookPrintList { get {  return StaticBookPrintList;  }set { OnPropertyChanged(); } }
        public ObservableCollection<PersonViewModel> UserIDPrintList { get { return StaticUserIDPrintList; } set { OnPropertyChanged(); } }


        //button commands
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }
        public DelegateCommand CloseApplicationCommand { get; set; }

        //print commands 
        public DelegateCommand PrintBookBarcodesCommand { get; set; }
        public DelegateCommand PrintUserCardsCommand { get; set; }

        //context menu commands
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }

        //SearchCommands
        public DelegateCommand SearchTitleInAllBooks { get; set; }
        public DelegateCommand ClearSearchInAllBooks { get; set; }
        public DelegateCommand SearchVornameInUsers { get; set; }
        public DelegateCommand ClearSearchInUsers { get; set; }
        public DelegateCommand SearchTitleInLendedBooks { get; set; }
        public DelegateCommand ClearSearchInLendedBooks { get; set; }

        //book searche commands
        public DelegateCommand SortTitleCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortISBNCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }

        //book search commands
        public DelegateCommand SortVornameCommand { get; set; }
        public DelegateCommand SortNachnameCommand { get; set; }
        public DelegateCommand SortRechteCommand { get; set; }

        //lended book commands
        public DelegateCommand SortLendedName { get; set; }
        public DelegateCommand SortLendedDate { get; set; }


        public Rechtelevel UserRights { get; }

        //searchword propertys
        public string AllBookSearchWord { get { return _allBookSearchWord; } set { _allBookSearchWord = value; OnPropertyChanged(); } }
        public string UserSearchWord { get { return _userSearchWord; } set { _userSearchWord = value; OnPropertyChanged(); } }
        public string LendedBooksSearchWord { get { return _lendedBooksSearchWord; } set { _lendedBooksSearchWord = value; OnPropertyChanged(); } }

        public ObservableCollection<PersonViewModel> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookViewModel> BookList
        {
            get
            {
                return _bookList;
            }
            set { SetValue(ref _bookList, value); }
        }
        public ObservableCollection<ExemplarViewModel> LendedBookList 
        { 
            get 
                { 
                return _lendedBookList; 
                } 
            set 
                {
                _lendedBookList = value; 
                OnPropertyChanged(); 
                } 
        }


        public AdminViewModel(INavigationService navigationService, ILibrary library, PersonModel user, Dispatcher dispatcher)
        {
            _navigationService = navigationService;
            _library = library;
            _user = user;
            _dispatcher = dispatcher;
            ScanUserViewModel.LogoutTimer.Enabled = false;

            _originalList = _library.GetAllBooks();
            _currentList = _originalList;
            UpdateBooklist(_currentList);
            LendedBookList = new ObservableCollection<ExemplarViewModel>();
            UpdateLendedBooklist(_originalList);

            _originalUserList = _library.GetAllUsers();
            _currentUserList = _originalUserList;
            UpdateUserlist(_currentUserList);

            UserRights = user.Rechte;

            BackCommand = new DelegateCommand( 
                x =>
                {
                    ScanUserViewModel.LogoutTimer.Enabled = true;
                    ScanUserViewModel.LogoutTimer.Start();
                    _navigationService.ShowLibraryView(user);
                });

            AddPersonCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddPersonView(user);
                }, 
                y =>
                {
                    return user.Rechte >= Rechtelevel.HELFER;
                });
            
            AddBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.HELFER;
                });

            PrintBookBarcodesCommand = new DelegateCommand(
                x =>
                {
                    var tupelList = new List<(string barcode, string name)>();

                    foreach (var book in BookPrintList)
                    {
                        tupelList.Add((book.Model.BasicInfos.Barcode, book.BuchModel.BasicInfos.Titel + " - " + book.BuchModel.BasicInfos.Author));
                    }
                    Utils.GenerateMultipleBarcodePDF(tupelList, false, user.VorUndNachname, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                    MessageBox.Show("Die PDF Datei wurde unter: " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\BoOp_PDF_Dateien\\{user.VorUndNachname}\\Bücher gespeichert.");
                    StaticBookPrintList.Clear();
                },
                y => 
                {
                    return BookPrintList.Count != 0;
                });

            PrintUserCardsCommand = new DelegateCommand(
                x =>
                {
                    var tupelList = new List<(string barcode, string name)>();

                    foreach (var user in UserIDPrintList)
                    {
                        tupelList.Add((user.Model.AusweisID, user.Model.VorUndNachname));
                    }
                    Utils.GenerateMultipleBarcodePDF(tupelList, true, user.VorUndNachname, Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

                    MessageBox.Show("Die PDF Datei wurde unter: " + Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\BoOp_PDF_Dateien\\{user.VorUndNachname}\\Ausweise gespeichert.");
                    StaticUserIDPrintList.Clear();
                },
                y =>
                {
                    return UserIDPrintList.Count != 0;
                });

            //SortTitleCommand
            SortTitleCommand = new DelegateCommand(
                x =>
                {
                    if (!_titleFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_currentList));
                        _titleFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //SortAuthorCommand
            SortAuthorCommand = new DelegateCommand(
                x =>
                {
                    if (!_authorFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByAuthor(_currentList));
                        _authorFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByAuthor(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //SortISBNCommand
            SortISBNCommand = new DelegateCommand(
                x =>
                {
                    if (!_isbnFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByISBN(_currentList));
                        _isbnFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByISBN(_currentList, true));
                        SetBookSortingFlagsFlase();
                    }
                });

            //Sort Vorname
            SortVornameCommand = new DelegateCommand(
                x =>
                {
                    if (!_vornameFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByVorname(_currentUserList));
                        _vornameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByVorname(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Nachname
            SortNachnameCommand = new DelegateCommand(
                x =>
                {
                    if (!_nachnameFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_currentUserList));
                        _nachnameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Rechte
            SortRechteCommand = new DelegateCommand(
                x =>
                {
                    if (!_rechteFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_currentUserList));
                        _rechteFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_currentUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            SortLendedDate = new DelegateCommand(
                x =>
                {
                    if (!_lendeddateflag)
                    {
                        SortLendedListByDate(true);
                        _lendeddateflag = true;
                    }
                    else
                    {
                        SortLendedListByDate();
                        SetLendedSortingFlagsFalse();
                    }
                });

            SortLendedName = new DelegateCommand(
                x =>
                {
                    if (!_lendednameflag)
                    {
                        SortLendedListByName(true);
                        _lendednameflag = true;
                    }
                    else
                    {
                        SortLendedListByName();
                        SetLendedSortingFlagsFalse();
                    }
                });

            SearchTitleInAllBooks = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(Utils.SearchForTitleInBooklist(_originalList, _allBookSearchWord));
                });

            ClearSearchInAllBooks = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(_originalList);
                    AllBookSearchWord = "";
                });

            SearchVornameInUsers = new DelegateCommand(
                x =>
                {
                    UpdateUserlist(Utils.SearchVornameInUserlist(_originalUserList, _userSearchWord));
                });

            ClearSearchInUsers = new DelegateCommand(
                x =>
                {
                    UpdateUserlist(_originalUserList);
                    UserSearchWord = "";
                });

            SearchTitleInLendedBooks = new DelegateCommand(
                x =>
                {
                    UpdateLendedBooklist(Utils.SearchForTitleInBooklist(_originalList, _lendedBooksSearchWord));
                });

            ClearSearchInLendedBooks = new DelegateCommand(
                x =>
                {
                    UpdateLendedBooklist(_originalList);
                    LendedBooksSearchWord = "";
                });

            CloseApplicationCommand = new DelegateCommand(
                x =>
                {
                    if (MessageBox.Show("Wollen Sie die Anwendung Schließen?", "Schließen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Environment.Exit(0);
                    }
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });
        }

        public List<ExemplarViewModel> GetLendedBooksFromUser(PersonModel user)
        {
            var list = new List<ExemplarViewModel>();
            foreach (var book in BookList)
            {
                foreach (var exemplar in book.Model.Exemplare)
                {
                    if (exemplar.LendBy != null)
                    {
                        if (user.AusweisID == exemplar.LendBy.AusweisID)
                        {
                            list.Add(new ExemplarViewModel(_navigationService, exemplar, book.Model));
                        }
                    }
                    
                }
            }
            return list;
        }

        private void UpdateBooklist(ObservableCollection<BuchModel> booklist)
        {
            _currentList = booklist;
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in booklist)
            {
                BookList.Add(new BookViewModel(book, _navigationService, _library, _user, this));
            }
        }

        private void UpdateUserlist(ObservableCollection<PersonModel> userlist)
        {
            _currentUserList = userlist;
            UserList = new ObservableCollection<PersonViewModel>();
            foreach (var user in userlist)
            {
                UserList.Add(new PersonViewModel(user, _navigationService, _library, _user, this));
            }
        }

        private void UpdateLendedBooklist(ObservableCollection<BuchModel> booklist)
        {
            LendedBookList = new ObservableCollection<ExemplarViewModel>();

            foreach (var book in booklist)
            {
                foreach (var exemplar in book.Exemplare)
                {
                    if (exemplar.LendBy != null)
                    {
                        LendedBookList.Add(new ExemplarViewModel(_navigationService, exemplar, book, this));
                    }
                }
            }
        }

        private void SetBookSortingFlagsFlase()
        {
            _isbnFlag = false;
            _authorFlag = false;
            _titleFlag = false; 
        }

        private void SetLendedSortingFlagsFalse()
        {
            _lendeddateflag = false; 
            _lendednameflag = false;
        }

        private void SetUserSortingFlagsFlase()
        {
            _vornameFlag = false;
            _nachnameFlag = false;
            _rechteFlag = false;
        }

        private void SortLendedListByDate(bool reverse = false)
        {
            ExemplarViewModel temp = null;

            for (int j = 0; j <= LendedBookList.Count - 2; j++)
            {
                for (int i = 0; i <= LendedBookList.Count - 2; i++)
                {
                    int comparison = DateTime.Compare(LendedBookList[i].Model.BasicInfos.AusleihDatum, LendedBookList[i + 1].Model.BasicInfos.AusleihDatum);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                }
            }
        }

        private void SortLendedListByName(bool reverse = false)
        {
            ExemplarViewModel temp = null;

            for (int j = 0; j <= LendedBookList.Count - 2; j++)
            {
                for (int i = 0; i <= LendedBookList.Count - 2; i++)
                {
                    int comparison = string.Compare(LendedBookList[i].Model.LendBy.Vorname + " " + LendedBookList[i].Model.LendBy.Nachname, LendedBookList[i + 1].Model.LendBy.Vorname + " " + LendedBookList[i].Model.LendBy.Nachname, comparisonType: StringComparison.InvariantCulture);

                    if (!reverse)
                    {
                        if (comparison > 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                    else
                    {
                        if (comparison < 0)
                        {
                            temp = LendedBookList[i + 1];
                            LendedBookList[i + 1] = LendedBookList[i];
                            LendedBookList[i] = temp;
                        }
                    }
                }
            }
        }
    }
}