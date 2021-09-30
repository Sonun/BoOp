using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.Business;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.ViewModels
{
    public class AddPersonViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public AddPersonViewModel(INavigationService navigationService, ILibrary library)
        {
            _library = library;
            _navigationService = navigationService;

            CancelCommand = new DelegateCommand(
                x => 
                {
                    _navigationService.ShowLibraryView();
                });
        }
    }
}
