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

        public AddBookViewModel AddBookViewModel { get; set; } 
        public AddPersonViewModel AddPersonViewModel { get; set; }
        public int BookDetailsPropertyNameWidth { get; set; } = 130;
        public string BookCoverPath { get; set; }

        public BookDetailsViewModel(INavigationService navigationService,PersonModel user, BuchModel buchModel)
        {
            BuchModel = buchModel;
            PersonModel = user;
            _navigationService = navigationService;
            AddBookViewModel = new AddBookViewModel(navigationService, null, PersonModel);
            AddPersonViewModel = new AddPersonViewModel(navigationService, null, user);
            SetBookCoverPath(buchModel);
        }

        private void SetBookCoverPath(BuchModel book)
        {
            var rawISBNSplit = book.BasicInfos.ISBN.Split(' ', '-', '.', ',');
            var rawISBN = string.Join("", rawISBNSplit);
            BookCoverPath = "http://covers.openlibrary.org/b/isbn/" + rawISBN + ".jpg";
        }
    }
}
