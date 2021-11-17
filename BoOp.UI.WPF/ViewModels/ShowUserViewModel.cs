using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ShowUserViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 10/11/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : View model für die benutzer infos view
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ShowUserViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly PersonModel _logedinUser;
        private readonly LibraryViewModel _libraryViewModel;
        private bool _showUserDetailsView;
        private bool _backToLibrary;

        public bool ShowUserDetailsView { get { return _showUserDetailsView; } set { _showUserDetailsView = value; OnPropertyChanged(); } }
        public PersonModel PersonModel { get; set; }
        public ObservableCollection<ExemplarViewModel> LendedBooks { get; set; }
        public DelegateCommand BackCommand { get; set; }

        public ShowUserViewModel()
        {
            ShowUserDetailsView = false;
        }
        public ShowUserViewModel(INavigationService navigationService, PersonModel editor,PersonModel user, List<ExemplarViewModel> exemplarViewModels, LibraryViewModel libraryViewModel = null, bool backToLibrary = false)
        {
            ShowUserDetailsView = true;
            _backToLibrary = backToLibrary;
            LendedBooks = new ObservableCollection<ExemplarViewModel>();
            PersonModel = user;
            _libraryViewModel = libraryViewModel;
            exemplarViewModels.ForEach(x => LendedBooks.Add(x));
            _navigationService = navigationService;
            _logedinUser = editor;

            BackCommand = new DelegateCommand(x =>
            {
                if (_backToLibrary)
                {
                    _libraryViewModel.ShowUserViewModel = new ShowUserViewModel();
                }
                else
                {
                    _navigationService.ShowAdminView(editor);
                }
            });
        }
    }
}
