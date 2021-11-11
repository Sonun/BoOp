using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    public class BookDetailsViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private LibraryViewModel _libraryViewModel;
        private ObservableCollection<ReviewViewModel> _reviewViewModels;
        private ObservableCollection<RatingViewModel> _ratings;
        private bool _ratingFlag;
        private bool _showBookDetailsView;
        private string _reviewText;

        public RatingViewModel SelectedRating { get; set; }
        public BuchModel BuchModel { get; set; }
        public PersonModel PersonModel { get; set; }
        public DelegateCommand RateBookCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }
        public ObservableCollection<ReviewViewModel> ReviewViewModels { get { return _reviewViewModels; } set { _reviewViewModels = value; OnPropertyChanged(); } }
        public ObservableCollection<RatingViewModel> Ratings { get { return _ratings; } set { _ratings = value; OnPropertyChanged(); } }

        public int BookDetailsPropertyNameWidth { get; set; } = 180;
        public string BookCoverPath { get; set; } = "https://i.pinimg.com/474x/31/63/49/3163495d3176cdff641c3e1b269a7a96--story-books-kid-books.jpg";
        public string ReviewText { get { return _reviewText; } set { _reviewText = value; OnPropertyChanged(); } }
        public bool ShowBookDetailsView { get { return _showBookDetailsView; } set { _showBookDetailsView = value; OnPropertyChanged(); } }

        /// <summary>
        /// Instance of empty constructor serves as a trigger for visibility in LibraryView
        /// </summary>
        public BookDetailsViewModel()
        {
            ShowBookDetailsView = false;
        }
        public BookDetailsViewModel(INavigationService navigationService, PersonModel user, BuchModel buchModel, LibraryViewModel libraryViewModel)
        {
            _reviewViewModels = new ObservableCollection<ReviewViewModel>();
            _ratings = new ObservableCollection<RatingViewModel>();
            BuchModel = buchModel;
            PersonModel = user;
            _navigationService = navigationService;
            _libraryViewModel = libraryViewModel;

            for (int i = 1; i < 6; i++)
            {
                Ratings.Add(new RatingViewModel(i));
            }
            SelectedRating = Ratings.Last();

            if (buchModel != null)
            {
                SetBookCoverPath(buchModel);
                ShowBookDetailsView = true;
            }
            if (buchModel.Rezensionen != null)
            {
                buchModel.Rezensionen.ForEach(x => ReviewViewModels.Add(new ReviewViewModel(x)));
            }

            CloseCommand = new DelegateCommand(x =>
            {
                // ToDo: LibraryView needs to be updated properly.
                // Update View is useless. Doesnt work.
                libraryViewModel.BookDetailsViewModel = new BookDetailsViewModel();
                libraryViewModel.UpdateView();
            });

            SortRatingCommand = new DelegateCommand(x =>
            {
                var sortedReviews = Utils.SortReviewsByRating(buchModel.Rezensionen, _ratingFlag);
                ReviewViewModels.Clear();
                sortedReviews.ForEach(x => ReviewViewModels.Add(new ReviewViewModel(x)));
                _ratingFlag = !_ratingFlag;
            });

            RateBookCommand = new DelegateCommand(x =>
            {
                var review = new RezensionModel()
                {
                    Author = user,
                    BasicInfos = new BasicRezensionenModel()
                    {
                        BuchID = buchModel.BasicInfos.Id.GetValueOrDefault(),
                        PersonID = user.Id.GetValueOrDefault(),
                        Rezensionstext = ReviewText,
                        Sterne = SelectedRating.Rating
                    }
                };
               
                libraryViewModel.Library.AddReview(review);
                libraryViewModel.BookList.Where(x => x.Model.BasicInfos.Id == BuchModel.BasicInfos.Id).First().Model.Rezensionen.Add(review);
                ReviewText = "";
                SelectedRating = Ratings.Last();

                // Check if user already rated the book before
                var checkDoupleReview = ReviewViewModels.Where(x => x.RezensionModel.Author.Id == user.Id).FirstOrDefault();
                if (checkDoupleReview != null)
                {
                    ReviewViewModels.Remove(checkDoupleReview);
                    var deleteFromBookList = libraryViewModel.BookList.Where(x => x.Model.BasicInfos.Id == BuchModel.BasicInfos.Id).First().Model.Rezensionen.Where(x => x.BasicInfos.PersonID == user.Id).FirstOrDefault();
                    libraryViewModel.BookList.Where(x => x.Model.BasicInfos.Id == BuchModel.BasicInfos.Id).First().Model.Rezensionen.Remove(deleteFromBookList);
                    MessageBox.Show("Sie haben das Buch bereits bewertet. Die Bewertung wurde aktualisiert.");
                }
                else
                {
                    ReviewViewModels.Add(new ReviewViewModel(review));
                    MessageBox.Show("Vielen Dank für die Bewertung.");
                }
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
