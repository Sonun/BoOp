using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    public class BookDetailsViewModel : ViewModel
    {
        public BuchModel BuchModel { get; set; }
        public PersonModel PersonModel { get; set; }
        private INavigationService _navigationService;
        private LibraryViewModel _libraryViewModel;

        public DelegateCommand RateBookCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }


        public int BookDetailsPropertyNameWidth { get; set; } = 130;
        public string BookCoverPath { get; set; } = "https://i.pinimg.com/474x/31/63/49/3163495d3176cdff641c3e1b269a7a96--story-books-kid-books.jpg";

        private bool _showBookDetailsView;
        public bool ShowBookDetailsView { get { return _showBookDetailsView; } set { _showBookDetailsView = value; OnPropertyChanged(); } }

        public BookDetailsViewModel()
        {
            ShowBookDetailsView = false;
        }
        public BookDetailsViewModel(INavigationService navigationService, PersonModel user, BuchModel buchModel, LibraryViewModel libraryViewModel)
        {
            BuchModel = buchModel;
            PersonModel = user;
            _navigationService = navigationService;
            _libraryViewModel = libraryViewModel;

            if (buchModel != null)
            {
                SetBookCoverPath(buchModel);
                ShowBookDetailsView = true;
            }

            CloseCommand = new DelegateCommand(x =>
            {
                libraryViewModel.BookDetailsViewModel = new BookDetailsViewModel();
            });
        }

        private void SetBookCoverPath(BuchModel book)
        {
            var rawISBNSplit = book.BasicInfos.ISBN.Split(' ', '-', '.', ',');
            var rawISBN = string.Join("", rawISBNSplit);
            BookCoverPath = "http://covers.openlibrary.org/b/isbn/" + rawISBN + ".jpg";
        }
    }
}
