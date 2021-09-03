using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Windows.Threading;

namespace BoOp.UI.WPF.ViewModels
{
    class MainWindowViewModel : ViewModel, INavigationService
    {
        private readonly Dispatcher _dispatcher; 
        private ViewModel _currentViewModel;

        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            private set { SetValue(ref _currentViewModel, value); }
        }

        public MainWindowViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            ShowScanUserView();
        }

        public void ShowScanUserView()
        {
            CurrentViewModel = new ScanUserViewModel(this, _dispatcher);
        }

        public void ShowLoginView()
        {
            CurrentViewModel = new LoginViewModel(this, 1); ;
        }

        public void ShowBookView()
        {
            CurrentViewModel = new BookViewModel(this);
        }
    }
}
