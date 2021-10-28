﻿using BoOp.Business;
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
    public class EditBookViewModel : ViewModel
    {
        private BuchModel _buchModel;
        public BuchModel BuchModel { get { return _buchModel; } set { _buchModel = value; OnPropertyChanged(); } }
        public PersonModel User { get; set; }

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

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
            BuchModel = buch;
            User = user;
            _navigationService = navigationService;
            _library = library;

            Schlagwoerter = "";
            Genres = "";
            Altersvorschlag = "";
            Regal = "";

            // Hard data - can't be edited: Labels in view
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
                        Regal = _regal
                    },
                    Exemplare = new List<ExemplarModel>(),
                };

                try
                {
                    _library.EditBookDetails(bookModel);
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
                    foreach (var exemplar in bookModel.Exemplare)
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

                    // ToDo: EditBookDetails by ID
                    _library.EditBookDetails(bookModel);
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