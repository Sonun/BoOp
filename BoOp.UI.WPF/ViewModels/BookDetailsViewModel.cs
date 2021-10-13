using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class BookDetailsViewModel : ViewModel
    {
        public BuchModel BuchModel { get; set; }
        public PersonModel PersonModel { get; set; }
        private INavigationService _navigationService;
        public DelegateCommand RateBook { get; set; }

        public BookDetailsViewModel(INavigationService navigationService,PersonModel user, BuchModel buchModel)
        {
            BuchModel = buchModel;
            PersonModel = user;
            _navigationService = navigationService;
        }
    }
}
