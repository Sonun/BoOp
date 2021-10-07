﻿using BoOp.Business;
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
        private ILibrary _library;

        //bookviewmodel booklist (binded by the view)
        private ObservableCollection<BookViewModel> _bookList;

        //buch model booklists, original and current
        private ObservableCollection<BuchModel> _currentList;
        private ObservableCollection<BuchModel> _originalList;
        private string _searchWord;

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

        public LibraryViewModel(INavigationService navigationService, ILibrary library, PersonModel user)
        {
            SetSortingFlagsFlase();

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
            _library = library;
            _originalList = _library.GetAllBooks();
            _currentList = _originalList;

            //fill boolist first time
            UpdateBooklist(_originalList);

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
                    UpdateBooklist(Utils.SearchForWordInBooklist(_originalList, _searchWord));
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
