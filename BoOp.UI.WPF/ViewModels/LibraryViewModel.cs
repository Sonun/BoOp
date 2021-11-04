using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace BoOp.UI.WPF.ViewModels
{
    public class LibraryViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public ILibrary Library { get; private set; }
        private readonly Dispatcher _dispatcher;
        public PersonModel LoggedInUser { get; set; }
        //bookviewmodel booklist (binded by the view)
        private ObservableCollection<BookViewModel> _bookList;

        //buch model booklists, original and current
        private ObservableCollection<BuchModel> _currentList;
        private ObservableCollection<BuchModel> _originalList;

        private BookDetailsViewModel _bookDetailsViewModel;
        public BookDetailsViewModel BookDetailsViewModel 
        { 
            get 
            { 
                return _bookDetailsViewModel;
            } 
            set 
            { 
                _bookDetailsViewModel = value;
                OnPropertyChanged();
            } 
        }

        private string _searchWord;
        private string _searchBy;

        //flags to enable reverse sorting
        private bool _titleFlag, _authorFlag, _isbnFlag, _ratingFlag;
        public DelegateCommand OpenLoginView { get; set; }
        public DelegateCommand SortTitleCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortISBNCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand ClearSearchCommand { get; set; }
        public DelegateCommand LendBookCommand { get; set; }
        public DelegateCommand ReturnBookCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        public string SearchBy { get { return _searchBy; } set { _searchBy = value; OnPropertyChanged(); } }

        public string SearchWord {
            get
            {
                return _searchWord;
            }
            set 
            {
                _searchWord = value;
                OnPropertyChanged();
            }}

        public ObservableCollection<BookViewModel> BookList { 
            get 
            { 
                return _bookList;
            } 
            set
            {
                _bookList = value;
                OnPropertyChanged();
            }}

        public LibraryViewModel(INavigationService navigationService, ILibrary library, PersonModel user, Dispatcher dispatcher)
        {
            SetSortingFlagsFlase();
            LoggedInUser = user;
            //check if navigationservice is null?
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            //open login view command
            OpenLoginView = new DelegateCommand(
                x =>
                {
                  _navigationService.ShowLoginView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            //create booklist from library
            Library = library;
            _dispatcher = dispatcher;
            _originalList = Library.GetAllBooks();
            _currentList = _originalList;
            SearchBy = "Titel";

            //fill boolist first time
            UpdateBooklist(_originalList);

            BookDetailsViewModel = new BookDetailsViewModel();

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
                        SetSortingFlagsFlase();
                        _titleFlag = false;
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
                        SetSortingFlagsFlase();
                        _authorFlag = false;
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
                        SetSortingFlagsFlase();
                        _isbnFlag = false;
                    }
                });

            //SortRatingCommand
            SortRatingCommand = new DelegateCommand(
                x =>
                {
                    if (!_ratingFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByRating(_currentList));
                        _ratingFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByRating(_currentList, true));
                        SetSortingFlagsFlase();
                        _ratingFlag = false;
                    }
                });

            SearchCommand = new DelegateCommand(
                x =>
                {
                switch (_searchBy.Split(" ")[1].ToLower())
                    {
                        case ("titel"):
                            UpdateBooklist(Utils.SearchForTitleInBooklist(_originalList, SearchWord));
                            break;
                        case ("author"):
                            UpdateBooklist(Utils.SearchForAuthorInBooklist(_originalList, SearchWord));
                            break;
                        case ("schlagwort"):
                            UpdateBooklist(Utils.SearchForSchlagwortInBooklist(_originalList, SearchWord));
                            break;
                        case ("genre"):
                            UpdateBooklist(Utils.SearchForGenreInBooklist(_originalList, SearchWord));
                            break;
                    }
                });

            ClearSearchCommand = new DelegateCommand(
                x =>
                {
                    UpdateBooklist(_originalList);
                    SearchWord = "";
                });

            LendBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowLendBookView(user);
                });

            ReturnBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowReturnLendBookView(user);
                });

            LogoutCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowScanUserView();
                });
        }

        /// <summary>
        /// Useless method
        /// </summary>
        public void UpdateView()
        {
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in _originalList)
            {
                BookList.Add(new BookViewModel(book, _navigationService, Library, this, LoggedInUser));
            }

        }


        private void UpdateBooklist(ObservableCollection<BuchModel> booklist)
        {
            _currentList = booklist;
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in booklist)
            {
                BookList.Add(new BookViewModel(book, _navigationService, Library, this , LoggedInUser));
            }
        }

        private void SetSortingFlagsFlase()
        {
            _ratingFlag = false;
            _isbnFlag = false;
            _authorFlag = false;
            _titleFlag = false;
        }
    }
}