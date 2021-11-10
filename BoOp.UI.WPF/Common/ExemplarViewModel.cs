using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.Common
{
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
                //_navigationService.ShowUserView(Model.LendBy, )
            });

            DeleteFromListCommand = new DelegateCommand(x =>
            {
                var deleteModel = AdminViewModel.StaticBookPrintList.SingleOrDefault(x => x.Model.BasicInfos.Barcode == Model.BasicInfos.Barcode);
                AdminViewModel.StaticBookPrintList.Remove(deleteModel);
            });
        }
    }
}
