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
        private PersonModel _user;
        private string _searchWord;
        private bool _titleFlag, _authorFlag, _isbnFlag, _vornameFlag, _rechteFlag, _nachnameFlag;

        public ObservableCollection<ExemplarViewModel> LendedBookList { get; set; }
        public Rechtelevel UserRights { get; }
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }
        public DelegateCommand SortTitleCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortISBNCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }
        public DelegateCommand SortVornameCommand { get; set; }
        public DelegateCommand SortNachnameCommand { get; set; }
        public DelegateCommand SortRechteCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand ClearSearchCommand { get; set; }

        public string SearchWord
        {
            get
            {
                return _searchWord;
            }
            set
            {
                _searchWord = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BuchModel> AllBooks
        {
            get
            {
                return _library.GetAllBooks();
            }
        }

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
            set
            {
                _bookList = value;
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

            _originalUserList = _library.GetAllUsers();
            _currentUserList = _originalUserList;
            UpdateUserlist(_currentUserList);

            UserRights = user.Rechte;
            LendedBookList = new ObservableCollection<ExemplarViewModel>();
            
            foreach (var book in BookList)
            {
                foreach (var exemplar in book.Model.Exemplare)
                {
                    if (exemplar.LendBy != null)
                    {
                        LendedBookList.Add(new ExemplarViewModel(exemplar, book.Model));
                    }
                }
            }

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
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });
            
            AddBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAddBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
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
                        UpdateUserlist(Utils.SortedUserlistByVorname(_originalUserList));
                        _vornameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByVorname(_originalUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Nachname
            SortNachnameCommand = new DelegateCommand(
                x =>
                {
                    if (!_nachnameFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_originalUserList));
                        _nachnameFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByNachname(_originalUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //Sort Rechte
            SortRechteCommand = new DelegateCommand(
                x =>
                {
                    if (!_rechteFlag)
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_originalUserList));
                        _rechteFlag = true;
                    }
                    else
                    {
                        UpdateUserlist(Utils.SortedUserlistByRechte(_originalUserList, true));
                        SetUserSortingFlagsFlase();
                    }
                });

            //suche der bücher
            SearchCommand = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(Utils.SearchForWordInBooklist(_originalList, _searchWord));
                });

            //suche rueckgaengig machen
            ClearSearchCommand = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(_originalList);
                    SearchWord = "";
                });
        }

        private void UpdateBooklist(ObservableCollection<BuchModel> booklist)
        {
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in booklist)
            {
                BookList.Add(new BookViewModel(book, _navigationService, null, _user));
            }
        }

        private void UpdateUserlist(ObservableCollection<PersonModel> userlist)
        {
            UserList = new ObservableCollection<PersonViewModel>();
            foreach (var user in userlist)
            {
                UserList.Add(new PersonViewModel(user, _navigationService, _user));
            }
        }

        private void SetBookSortingFlagsFlase()
        {
            _isbnFlag = false;
            _authorFlag = false;
            _titleFlag = false;
        }

        private void SetUserSortingFlagsFlase()
        {
            _vornameFlag = false;
            _nachnameFlag = false;
            _rechteFlag = false;
        }
    }
}