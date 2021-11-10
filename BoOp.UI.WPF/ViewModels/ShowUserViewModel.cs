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
    public class ShowUserViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly PersonModel _logedinUser;

        public PersonModel PersonModel { get; set; }
        public ObservableCollection<ExemplarViewModel> LendedBooks { get; set; }
        public DelegateCommand BackCommand { get; set; }    

        public ShowUserViewModel(INavigationService navigationService, PersonModel editor,PersonModel user, List<ExemplarViewModel> exemplarViewModels)
        {
            LendedBooks = new ObservableCollection<ExemplarViewModel>();
            PersonModel = user;
            exemplarViewModels.ForEach(x => LendedBooks.Add(x));
            _navigationService = navigationService;
            _logedinUser = editor;

            BackCommand = new DelegateCommand(x =>
            {
                _navigationService.ShowAdminView(editor);
            });
        }
    }
}
