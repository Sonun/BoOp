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
    public class EditBookViewModel : ViewModel
    {
        private BuchModel _buchModel;
        public BuchModel BuchModel { get { return _buchModel; } set { _buchModel = value; OnPropertyChanged(); } }
        public PersonModel User { get; set; }
        public ObservableCollection<ExemplarViewModel> ExemplarViewModels { get; set; }

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand AddPrintListCommand { get; set; }
        public DelegateCommand DeleteBooksCommand { get; set; }

        private string _titel;
        private string _author;
        private string _verlag;
        private int? _auflage;
        private string _isbn;
        private string _altersvorschlag;
        private string _regal;
        private int _exemplare;
        private string _schlagwoerter;
        private string _genres;
        private readonly INavigationService _navigationService;
        private readonly ILibrary _library;

        public string Titel { get { return _titel; } set { _titel = value; OnPropertyChanged(); } }
        public string Author { get { return _author; } set { _author = value; OnPropertyChanged(); } }
        public string Verlag { get { return _verlag; } set { _verlag = value; OnPropertyChanged(); } }
        public int? Auflage { get { return _auflage; } set { _auflage = value; OnPropertyChanged(); } }
        public string ISBN { get { return _isbn; } set { _isbn = value; OnPropertyChanged(); } }
        public string Altersvorschlag { get { return _altersvorschlag; } set { _altersvorschlag = value; OnPropertyChanged(); } }
        public string Regal { get { return _regal; } set { _regal = value; OnPropertyChanged(); } }
        public int Exemplare { get { return _exemplare; } set { _exemplare = value; OnPropertyChanged(); } }
        public string Schlagwoerter { get { return _schlagwoerter; } set { _schlagwoerter = value; OnPropertyChanged(); } }
        public string Genres { get { return _genres; } set { _genres = value; OnPropertyChanged(); } }

        public EditBookViewModel(BuchModel buch, PersonModel user, INavigationService navigationService, ILibrary library)
        {
            ExemplarViewModels = new ObservableCollection<ExemplarViewModel>();
            BuchModel = buch;
            User = user;
            _navigationService = navigationService;
            _library = library;

            BuchModel.Exemplare.ForEach(x => ExemplarViewModels.Add(new ExemplarViewModel(x)));

            Schlagwoerter = "";
            Genres = "";
            Altersvorschlag = "";
            Regal = "";

            Titel = buch.BasicInfos.Titel;
            Author = buch.BasicInfos.Author;
            Verlag = buch.BasicInfos.Verlag;
            ISBN = buch.BasicInfos.ISBN;

            Auflage = buch.BasicInfos.Auflage.GetValueOrDefault();
            Altersvorschlag = buch.BasicInfos.Altersvorschlag;
            Regal = buch.BasicInfos.Regal;            
            buch.Schlagwoerter.ForEach(x => Schlagwoerter += x + " ");
            buch.Genres.ForEach(x => Genres += x + " ");
            Exemplare = buch.Exemplare.Count();

            // ToDo: Implement methods

            AddPrintListCommand = new DelegateCommand(x =>
            {
                var selectedExemplare = ExemplarViewModels.Where(x => x.IsChecked == true).ToList();

                selectedExemplare.ForEach(x => 
                {
                    var checkExemplar = AdminViewModel.StaticBookPrintList.SingleOrDefault(y => y.Model.BasicInfos.Barcode == x.Model.BasicInfos.Barcode);
                    if (checkExemplar == null)
                    {
                        x.BuchModel = BuchModel;
                        AdminViewModel.StaticBookPrintList.Add(x);
                    }
                    
                });
            });

            DeleteBooksCommand = new DelegateCommand(x =>
            {
                var selectedExemplare = ExemplarViewModels.Where(x => x.IsChecked == true).ToList();
                if (selectedExemplare.Count == BuchModel.ExemplarAnzahl)
                {
                    MessageBox.Show("Es können nicht alle Exemplare eines Buches gelöscht werden.");
                    return;
                }
                try
                {
                    selectedExemplare.ForEach(x => 
                    { 
                        library.RemoveExemplar(x.Model); 
                        ExemplarViewModels.Remove(x);
                        BuchModel.Exemplare.Remove(x.Model);
                    });
                    Exemplare = BuchModel.Exemplare.Count();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
                OnPropertyChanged(nameof(Exemplare));
            });

            CancelCommand = new DelegateCommand(x =>
            {
                _navigationService.ShowAdminView(user);
            });

            SaveCommand = new DelegateCommand(x =>
            {
                var schlagwoerter = _schlagwoerter.Split(' ', ',', '.', ';').ToList();
                schlagwoerter.RemoveAll(x => x == "");
                var genres = _genres.Split(' ', ',', '.', ';').ToList();
                genres.RemoveAll(x => x == "");

                if (_titel.Equals("") || _author.Equals("") || _verlag.Equals(""))
                {
                    MessageBox.Show("Füllen Sie Alle Felder Aus");
                    return;
                }
                BuchModel.BasicInfos.Titel = Titel = Titel.Trim(' ');
                BuchModel.BasicInfos.Author = Author.Trim(' ');
                BuchModel.BasicInfos.Auflage = _auflage;
                BuchModel.BasicInfos.Verlag = _verlag.Trim(' ');
                BuchModel.BasicInfos.Altersvorschlag = _altersvorschlag;
                BuchModel.BasicInfos.Regal = _regal;

                var bookCount = BuchModel.ExemplarAnzahl;

                if (_exemplare - BuchModel.ExemplarAnzahl > 0)
                {
                    for (int i = 0; i < _exemplare - bookCount; i++)
                    {
                        BuchModel.Exemplare.Add(
                            new ExemplarModel() 
                            { 
                                BasicInfos = new BasicExemplarModel() 
                                { 
                                    Barcode = ""
                                } 
                            });
                        BuchModel.Exemplare.Last().BasicInfos.Barcode = Utils.GenerateUniqueBarcode(BuchModel);
                    }
                }
                if (_exemplare - BuchModel.ExemplarAnzahl < 0)
                {
                    for (int i = 1; i <= bookCount - _exemplare; i++)
                    {
                        BuchModel.Exemplare.RemoveAt(BuchModel.Exemplare.Count - 1);
                    }
                }
                try
                {
                    if (schlagwoerter.Count > 0)
                    {
                        BuchModel.Schlagwoerter = schlagwoerter;
                    }
                    if (genres.Count > 0)
                    {
                        BuchModel.Genres = genres;
                    }

                    // EditBookDetails by ID
                    _library.EditBookDetails(BuchModel, true);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                _navigationService.ShowAdminView(user);
            });
        }
    }
}
