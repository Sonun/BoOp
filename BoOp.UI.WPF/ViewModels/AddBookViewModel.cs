﻿using BoOp.Business;
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

        public string Titel { get { return _titel; } set { _titel = value; OnPropertyChanged(); } }
        public string Author { get { return _author; } set { _author = value; OnPropertyChanged(); } }
        public string Verlag { get { return _verlag; } set { _verlag = value; OnPropertyChanged(); } }
        public int? Auflage { get { return _auflage; } set { _auflage = value; OnPropertyChanged(); } }
        public string ISBN { get { return _isbn; } set { _isbn = value; OnPropertyChanged(); } }
        public string Altersvorschlag { get { return _altersvorschlag; } set { _altersvorschlag = value; OnPropertyChanged(); } }
        public string Regal { get { return _regal; } set { _regal = value; OnPropertyChanged(); } }
        public int Exemplare { get { return _exemplare; } set { _exemplare = value; OnPropertyChanged(); } }

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
            Auflage = 1;
            ISBN = "";
            Altersvorschlag = "0";
            Regal = "";
            Exemplare = 1;

            SaveCommand = new DelegateCommand( 
                x =>
                {
                    if (_titel.Equals("") || _author.Equals("") || _verlag.Equals("") || _isbn.Equals(""))
                    {
                        MessageBox.Show("Füllen Sie Alle Felder Aus");
                        return;
                    }

                    var basicModel = new BasicBuchModel
                    {
                        Titel = _titel,
                        Auflage = _auflage,
                        Author = _author,
                        Verlag = _verlag, 
                        ISBN = _isbn,
                        Altersvorschlag = _altersvorschlag,
                        Regal = _regal
                    };

                    var exList = new List<ExemplarModel>();
                    var bookModel = new BuchModel { BasicInfos = basicModel, Exemplare = exList};

                    for (int i = 1; i <= _exemplare; i++)
                    {
                        bookModel.Exemplare.Add(new ExemplarModel() { BasicInfos = new BasicExemplarModel() { Barcode = ""} });
                    }

                    try
                    {
                        _library.AddBook(bookModel);
                        bookModel.BasicInfos.Id = _library.GetIdByISBN(bookModel);

                        foreach(var exemplar in bookModel.Exemplare)
                        {
                            exemplar.BasicInfos.Barcode = Utils.GenerateUniqueBarcode(bookModel);
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
