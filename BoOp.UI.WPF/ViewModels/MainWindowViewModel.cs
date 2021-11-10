using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace BoOp.UI.WPF.ViewModels
{
    class MainWindowViewModel : ViewModel, INavigationService
    {
        private readonly Dispatcher _dispatcher;
        private ViewModel _currentViewModel;
        private ILibrary _library;

        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            private set { SetValue(ref _currentViewModel, value); }
        }

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _library = new Library();
            ShowScanUserView();
        }

        public MainWindowViewModel(Dispatcher dispatcher, bool showDifferntView)
        {
            _dispatcher = dispatcher;
            _library = new Library();
        }

        public void ShowScanUserView()
        {
            CurrentViewModel = new ScanUserViewModel(this, _library, _dispatcher);
        }

        public void ShowAdminView(PersonModel user)
        {
            CurrentViewModel = new AdminViewModel(this, _library, user, _dispatcher);
        }

        public void ShowLoginView(PersonModel user)
        {
            CurrentViewModel = new LoginViewModel(this, _library, user); ;
        }

        public void ShowLibraryView(PersonModel user)
        {
            CurrentViewModel = new LibraryViewModel(this, _library, user, _dispatcher);
        }

        public void ShowAddPersonView(PersonModel user)
        {
            CurrentViewModel = new AddPersonViewModel(this, _library, user);
        }

        public void ShowAddBookView(PersonModel user)
        {
            CurrentViewModel = new AddBookViewModel(this, _library, user);
        }

        public void ShowEditUserView(PersonModel editor, PersonModel userToChange)
        {
            CurrentViewModel = new EditPersonViewModel(this, _library, editor, userToChange);
        }

        public void ShowLendBookView(PersonModel user)
        {
            CurrentViewModel = new LendBookViewModel(this, _library, user);
        }

        public void ShowReturnLendBookView(PersonModel user)
        {
            CurrentViewModel = new ReturnBookViewModel(this, _library, user);
        }


        public void ShowEditBookView(PersonModel user, BuchModel book, AdminViewModel adminViewModel)
        {
            CurrentViewModel = new EditBookViewModel(book, user, this, _library, adminViewModel);
        }

        public void ShowUserView(List<ExemplarViewModel> exemplare, PersonModel user, PersonModel logedinUser)
        {
            CurrentViewModel = new ShowUserViewModel(this, logedinUser, user, exemplare);
        }
    }
}
