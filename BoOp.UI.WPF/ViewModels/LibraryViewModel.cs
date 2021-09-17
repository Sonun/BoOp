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

namespace BoOp.UI.WPF.ViewModels
{
    public class LibraryViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ObservableCollection<BookViewModel> _bookList;
        private ObservableCollection<BuchModel> _originalList;
        private bool _titleFlag, _authorFlag, _isbnFlag, _ratingFlag;

        public DelegateCommand OpenLoginView { get; set; }
        public DelegateCommand SortTitleCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortISBNCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }

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

        public LibraryViewModel(INavigationService navigationService)
        {
            SetSortingFlagsFlase();

            //check if navigationservice is null?
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            //open login view command
            OpenLoginView = new DelegateCommand(
                x =>
                {
                  _navigationService.ShowLoginView();
                });

            //create booklist from library
            Library lib = new Library();
            _originalList = lib.GetAllBooks();

            //fill boolist first time
            UpdateBooklist(_originalList);

            //SortTitleCommand
            SortTitleCommand = new DelegateCommand(
                x => 
                {
                    if (!_titleFlag)
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_originalList));
                        _titleFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByTitel(_originalList, true));
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
                        UpdateBooklist(Utils.SortedBookListByAuthor(_originalList));
                        _authorFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByAuthor(_originalList, true));
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
                        UpdateBooklist(Utils.SortedBookListByISBN(_originalList));
                        _isbnFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByISBN(_originalList, true));
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
                        UpdateBooklist(Utils.SortedBookListByRating(_originalList));
                        _ratingFlag = true;
                    }
                    else
                    {
                        UpdateBooklist(Utils.SortedBookListByRating(_originalList, true));
                        SetSortingFlagsFlase();
                        _ratingFlag = false;
                    }
                });
        }

        private void UpdateBooklist(ObservableCollection<BuchModel> _booklist)
        {
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in _booklist)
            {
                BookList.Add(new BookViewModel(book));
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
