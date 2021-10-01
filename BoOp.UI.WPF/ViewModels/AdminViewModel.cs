using BoOp.Business;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class AdminViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand AddPersonCommand { get; set; }
        public DelegateCommand LendBookCommand { get; set; }
        public DelegateCommand ReturnBookCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand AddUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand EditUserCommand { get; set; }

        public AdminViewModel(INavigationService navigationService, ILibrary library)
        {
            _navigationService = navigationService;
            _library = library;

            BackCommand = new DelegateCommand( 
                x =>
                {
                    _navigationService.ShowLibraryView();
                });

            AddPersonCommand = new DelegateCommand( 
                x =>
                {
                    _navigationService.ShowAddPersonView();
                });
        }
    }
}
