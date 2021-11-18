using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Linq;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ExemplarViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 3/11/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : View Model für exemplare
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ExemplarViewModel : ViewModel
    {
        private bool _isChecked;
        private readonly INavigationService _navigationService;

        public  AdminViewModel AdminViewModel { get; private set; }

        public ExemplarModel Model { get; set; }
        public BuchModel BuchModel { get; set; }
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; OnPropertyChanged(); } }
        public DelegateCommand ShowUserCommand { get; set; }
        public DelegateCommand DeleteFromListCommand { get; set; }


        public ExemplarViewModel(INavigationService navigationService, ExemplarModel model, BuchModel buch, AdminViewModel adminViewModel = null)
        {
            _navigationService = navigationService;
            Model = model;
            BuchModel = buch;
            AdminViewModel = adminViewModel;
            
            ShowUserCommand = new DelegateCommand(x =>
            {
                var userBooks = adminViewModel.GetLendedBooksFromUser(model.LendBy);
                _navigationService.ShowUserView(userBooks, model.LendBy, adminViewModel.LoggedinUser);
            });

            DeleteFromListCommand = new DelegateCommand(x =>
            {
                var deleteModel = AdminViewModel.StaticBookPrintList.SingleOrDefault(x => x.Model.BasicInfos.Barcode == Model.BasicInfos.Barcode);
                AdminViewModel.StaticBookPrintList.Remove(deleteModel);
            });
        }
    }
}
