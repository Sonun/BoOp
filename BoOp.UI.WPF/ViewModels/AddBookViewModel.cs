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
    public class AddBookViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;

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


        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public AddBookViewModel(INavigationService navigationservice, ILibrary library, PersonModel user)
        {
            _navigationService = navigationservice;
            if (user.Rechte < Rechtelevel.BIBOTEAM)
            {
                _navigationService.ShowLibraryView(user);
                return;
            }

            _library = library;

            Titel = "";
            Author = "";
            Verlag = "";
            ISBN = "";
            Schlagwoerter = "";
            Genres = "";
            Exemplare = 1;

            SaveCommand = new DelegateCommand( 
                x =>
                {
                    var schlagwoerter = _schlagwoerter.Split(' ', ',', '.', ';').ToList();
                    schlagwoerter.RemoveAll(x => x == "");
                    var genres = _genres.Split(' ', ',', '.', ';').ToList();
                    genres.RemoveAll(x => x == "");

                    if (_titel.Equals("") || _author.Equals("") || _verlag.Equals("") || _isbn.Equals(""))
                    {
                        MessageBox.Show("Füllen Sie Alle Felder Aus");
                        return;
                    }

                    var bookModel = new BuchModel 
                    { 
                        BasicInfos = new BasicBuchModel
                        {
                            Titel = _titel,
                            Auflage = _auflage,
                            Author = _author,
                            Verlag = _verlag,
                            ISBN = _isbn,
                            Altersvorschlag = _altersvorschlag,
                            Regal = _regal
                        }, 
                        Exemplare = new List<ExemplarModel>(),
                    };

                    _library.AddBook(bookModel);
                    bookModel.BasicInfos.Id = _library.GetIdByISBN(bookModel);


                    for (int i = 1; i <= _exemplare; i++)
                    {
                        bookModel.Exemplare.Add(new ExemplarModel() { BasicInfos = new BasicExemplarModel() { Barcode = "" } });
                    }

                    try
                    {
                        foreach(var exemplar in bookModel.Exemplare)
                        {
                            exemplar.BasicInfos.Barcode = Utils.GenerateUniqueBarcode(bookModel);
                        }
                        if (schlagwoerter.Count > 0)
                        {
                            bookModel.Schlagwoerter = schlagwoerter;
                        }
                        if (genres.Count > 0)
                        {
                            bookModel.Genres = schlagwoerter;
                        }

                        _library.EditBookDetails(bookModel);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return;
                    }

                    _navigationService.ShowAdminView(user);
                });

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAdminView(user);
                });
        }
    }
}
