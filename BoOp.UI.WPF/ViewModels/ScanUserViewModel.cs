using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class ScanUserViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private string _status;

        public DelegateCommand TestButtonCommand { get; set; }

        public string Status {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ScanUserViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Status = "Bitte Scann deine Karte\n   mit dem Kartenleser!";

            TestButtonCommand = new DelegateCommand
                (
                     x =>
                     {
                         _navigationService.ShowLoginView();
                     }
                );
        }
    }
}
