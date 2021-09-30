using BoOp.Business;
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

        public void ShowAdminView()
        {
            CurrentViewModel = new AdminViewModel(this, _library);
        }

        public void ShowLoginView()
        {
            CurrentViewModel = new LoginViewModel(this, _library, 1); ;
        }

        public void ShowLibraryView()
        {
            CurrentViewModel = new LibraryViewModel(this, _library);
        }

        public void ShowAddPersonView()
        {
            CurrentViewModel = new AddPersonViewModel(this, _library);
        }
    }
}
