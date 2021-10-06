using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
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

        public void ShowScanUserView()
        {
            CurrentViewModel = new ScanUserViewModel(this, _library, _dispatcher);
        }

        public void ShowAdminView(PersonModel user)
        {
            CurrentViewModel = new AdminViewModel(this, _library, user);
        }

        public void ShowLoginView(PersonModel user)
        {
            CurrentViewModel = new LoginViewModel(this, _library, user); ;
        }

        public void ShowLibraryView(PersonModel user)
        {
            CurrentViewModel = new LibraryViewModel(this, _library, user);
        }

        public void ShowAddPersonView(PersonModel user)
        {
            CurrentViewModel = new AddPersonViewModel(this, _library, user);
        }

        public void ShowAddBookView(PersonModel user)
        {
            CurrentViewModel = new AddBookViewModel(this, _library, user);
        }

        public void ShowEditUserView(PersonModel user)
        {
            CurrentViewModel = new EditPersonViewModel(this, _library, user);
        }
    }
}
