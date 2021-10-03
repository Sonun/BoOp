using BoOp.Business;
using BoOp.DBAccessor.Models;
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
        public DelegateCommand AddBookCommand { get; set; }

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
            AddBookCommand = new DelegateCommand(
                x =>
                {
                    library.AddBook(new BuchModel()
                    {
                        BasicInfos = new BasicBuchModel() { Altersvorschlag = "ab 12", Author = "Domi", Titel = "Das Leben.", Verlag = "Selfmade", Regal = "5A", ISBN = "123213-123123-123" },
                        Schlagwoerter = new List<string>() { "Easy", "Krass" },
                        Genres = new List<string>() { "Drama", "Horror" },
                        Exemplare = new List<ExemplarModel>()
                        {
                            new ExemplarModel(){ BasicInfos = new BasicExemplarModel(){ Barcode = "BoOp.12123.213213" }},
                            new ExemplarModel(){ BasicInfos = new BasicExemplarModel(){ Barcode = "BoOp.987654321" }}

                        }

                    }) ;
                });
        }
    }
}
