using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

namespace BoOp.UI.WPF.ViewModels
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: AddBookViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 4/10/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Viewmodel für die Buch hinzufügen View
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class AddBookViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private ILibrary _library;
        private WebClient _webClient;

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
        private string _bookDetailsJSONLink = "";
        private bool _addPrintList;
        private string _beschreibung;

        private string _coverPath;

        public string CoverPath
        {
            get { return _coverPath; }
            set { SetValue(ref _coverPath, value); }
        }


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
        public string Beschreibung { get { return _beschreibung; } set { _beschreibung = value; OnPropertyChanged(); } }
        public bool AddPrintList { get { return _addPrintList; } set { _addPrintList = value; OnPropertyChanged(); } }
        
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SearchInfosCommand { get; set; }
        public DelegateCommand LoadPicCommand { get; set; }

        public AddBookViewModel(INavigationService navigationservice, ILibrary library, PersonModel user)
        {
            _navigationService = navigationservice;
            _webClient = new WebClient();

            _library = library;
            AddPrintList = true;

            Titel = "";
            Author = "";
            Verlag = "";
            ISBN = "";
            Schlagwoerter = "";
            Genres = "";
            Beschreibung = "";
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

                    var isbn = new string(ISBN.Trim(' ').ToArray());


                    var bookModel = new BuchModel
                    {
                        BasicInfos = new BasicBuchModel
                        {
                            Titel = Titel.Trim(' '),
                            Auflage = _auflage,
                            Author = Author.Trim(' '),
                            Verlag = _verlag.Trim(' '),
                            ISBN = new string(_isbn.Where(c => char.IsDigit(c)).ToArray()),
                            Altersvorschlag = _altersvorschlag,
                            Regal = _regal,
                            Beschreibung = _beschreibung
                        }, 
                        Exemplare = new List<ExemplarModel>(),
                    };

                    try
                    {
                        _library.AddBook(bookModel);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return;
                    }

                    bookModel.BasicInfos.Id = _library.GetIdByISBN(bookModel);


                    for (int i = 1; i <= _exemplare; i++)
                    {
                        bookModel.Exemplare.Add(new ExemplarModel() { BasicInfos = new BasicExemplarModel() { Barcode = "" } });
                    }

                    try
                    {
                        foreach(var exemplar in bookModel.Exemplare)
                        {
                            exemplar.BasicInfos.Barcode = Utils.GenerateUniqueBookBarcodeString(bookModel);
                        }
                        if (schlagwoerter.Count > 0)
                        {
                            bookModel.Schlagwoerter = schlagwoerter;
                        }
                        if (genres.Count > 0)
                        {
                            bookModel.Genres = genres;
                        }

                        _library.EditBookDetails(bookModel);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return;
                    }
                    if (AddPrintList)
                    {
                        bookModel.Exemplare.ForEach(x => AdminViewModel.StaticBookPrintList.Add(new ExemplarViewModel(navigationservice, x, bookModel)));
                    }

                    _navigationService.ShowAdminView(user);
                });

            CancelCommand = new DelegateCommand(
                x =>
                {
                    _navigationService.ShowAdminView(user);
                });

            SearchInfosCommand = new DelegateCommand(
                x =>
                {
                    try
                    {
                        SetBookDetailsJSONLink();
                    }
                    catch
                    {
                        MessageBox.Show("ISBN wurde nicht gefunden, bitte geben sie das Buch händisch ein!");
                    }
                });
            LoadPicCommand = new DelegateCommand(
                x =>
                {
                    try
                    {

                    }
                    catch
                    {

                    }
                });
        }


        private void SetBookDetailsJSONLink()
        {
            var rawISBNSplit = _isbn.Split(' ', '-', '.', ',');
            var rawISBN = string.Join("", rawISBNSplit);
            ISBN = rawISBN;
            _bookDetailsJSONLink = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + rawISBN;

            var json = _webClient.DownloadString(_bookDetailsJSONLink);
            var sortedInfos = json.Split("\n").ToList();
            var selfLink = new string(sortedInfos.Where(x => x.Contains("selfLink")).FirstOrDefault().SkipWhile(x => x != 'h').TakeWhile(x=> x != '"').ToArray());

            json = _webClient.DownloadString(selfLink);
            sortedInfos = json.Split("\n").ToList();

            Titel = new string(sortedInfos.Where(x => x.Contains("title")).FirstOrDefault().Trim().Skip(10).TakeWhile(x => x != '"').ToArray());

            var indexAuthor = sortedInfos.FindIndex(x => x.Contains("authors"));
            Author = new string(sortedInfos[indexAuthor + 1].Trim().Skip(1).TakeWhile(x => x != '"').ToArray());

            Verlag = new string(sortedInfos.Where(x => x.Contains("publisher")).FirstOrDefault().Trim().Skip(14).TakeWhile(x => x != '"').ToArray());
        }
    }
}
