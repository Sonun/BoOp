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
        private PersonModel _user;

        public Rechtelevel UserRights { get; }
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }
        public DelegateCommand SortAuthorCommand { get; set; }
        public DelegateCommand SortTitleCommand { get; set; }
        public BookViewModel SelectedBook { get; set; }


        public ObservableCollection<BuchModel> AllBooks
        {
            get
            {
                return _library.GetAllBooks();
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
            UpdateBooklist( _library.GetAllBooks());
            UserRights = user.Rechte;

            BackCommand = new DelegateCommand( 
                x =>
                {
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

            EditUserCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowEditUserView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            EditBookCommand = new DelegateCommand(
                x =>
                {

                });

            RemoveBookCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowRemoveBookView(user);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.ADMIN;
                });
        }

        private void UpdateBooklist(ObservableCollection<BuchModel> _booklist)
        {
            BookList = new ObservableCollection<BookViewModel>();
            foreach (var book in _booklist)
            {
                BookList.Add(new BookViewModel(book, _dispatcher, _user));
            }
        }
    }
}