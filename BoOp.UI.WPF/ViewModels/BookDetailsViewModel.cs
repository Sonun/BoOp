using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: BookDetailsViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 12/10/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Viewmodel für die book details view
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BookDetailsViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private LibraryViewModel _libraryViewModel;
        private ObservableCollection<ReviewViewModel> _reviewViewModels;
        private ObservableCollection<RatingViewModel> _ratings;
        private bool _ratingFlag;
        private bool _showBookDetailsView;
        private string _reviewText;
        private double _selectedRating;
        private string _coverPath;

        public double Rating
        {
            get
            {
                return _selectedRating;
            }
            set
            {
                _selectedRating = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RatingPic));
                SelectedRating = new RatingViewModel(_selectedRating);
            }
        }

        public RatingViewModel SelectedRating { get; set; }

        public BuchModel BuchModel { get; set; }
        public PersonModel PersonModel { get; set; }
        public DelegateCommand RateBookCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand SortRatingCommand { get; set; }
        public ObservableCollection<ReviewViewModel> ReviewViewModels { get { return _reviewViewModels; } set { _reviewViewModels = value; OnPropertyChanged(); } }
        public ObservableCollection<RatingViewModel> Ratings { get { return _ratings; } set { _ratings = value; OnPropertyChanged(); } }

        public string RatingPic
        {
            get
            {
                switch (Rating)
                {
                    case (1):
                        return "/Assets/Butterflies/1butterflies.png";
                    case (< 2):
                        return "/Assets/Butterflies/1.5butterflies.png";
                    case (< 2.5):
                        return "/Assets/Butterflies/2butterflies.png";
                    case (< 3):
                        return "/Assets/Butterflies/2.5butterflies.png";
                    case (< 3.5):
                        return "/Assets/Butterflies/3butterflies.png";
                    case (< 4):
                        return "/Assets/Butterflies/3.5butterflies.png";
                    case (< 4.5):
                        return "/Assets/Butterflies/4butterflies.png";
                    case (< 5):
                        return "/Assets/Butterflies/4.5butterflies.png";
                    case (< 5.5):
                        return "/Assets/Butterflies/5butterflies.png";
                    default:
                        return "/Assets/Butterflies/0butterflies.png";
                }
            }
        }

        public string Genres
        {
            get
            {
                var genreString = "";
                if (BuchModel != null)
                {
                    foreach (var genre in BuchModel.Genres)
                    {
                        genreString += genre + " ";
                    }
                }
                return genreString;
            }
        }
        public string Schlagwoerter
        {
            get
            {
                var schlagwoerter = "";
                if (BuchModel != null)
                {
                    foreach (var wort in BuchModel.Schlagwoerter)
                    {
                        schlagwoerter += wort + " ";
                    }
                }
                return schlagwoerter;
            }
        }

        public int BookDetailsPropertyNameWidth { get; set; } = 180;
        public string CoverPath
        {
            get { return _coverPath; }
            set { SetValue(ref _coverPath, value); }
        }
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
            CoverPath = "/Assets/Icons/Review/nocover.png";

            for (int i = 1; i < 6; i++)
            {
                Ratings.Add(new RatingViewModel(i));
            }
            SelectedRating = Ratings.Last();
            Rating = 3;

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
                    ReviewViewModels.Add(new ReviewViewModel(review));
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

            if (book.BasicInfos.BildPfad != null)
            {
                if (book.BasicInfos.BildPfad.Trim() != "")
                {
                    CoverPath = book.BasicInfos.BildPfad;
                }
                else
                {
                    CoverPath = "http://covers.openlibrary.org/b/isbn/" + rawISBN + ".jpg";
                }
            }
            else
            {
                CoverPath = "http://covers.openlibrary.org/b/isbn/" + rawISBN + ".jpg";
            }
        }
    }
}
