using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System.Linq;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;
using BoOp.Business;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: BookViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 16/9/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : Buch View Model und dazu gehörige methoden
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class BookViewModel : ViewModel
    {
        public BuchModel Model { get; set; }

        private readonly ILibrary _library;
        private readonly LibraryViewModel _libraryViewModel;
        private readonly AdminViewModel _adminViewModel;

        public DelegateCommand ShowBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }

        public BookViewModel(BuchModel model, INavigationService navigationService, ILibrary library, PersonModel user, AdminViewModel adminViewModel = null, LibraryViewModel libraryViewModel = null)
        {
            Model = model;
            _library = library;
            _libraryViewModel = libraryViewModel;
            _adminViewModel = adminViewModel;

            ShowBookCommand = new DelegateCommand(
                x => 
                {
                    _libraryViewModel.BookDetailsViewModel = new BookDetailsViewModel(navigationService, user, model, libraryViewModel);
                },
                y => 
                { 
                    return user != null;
                });

            EditBookCommand = new DelegateCommand(
                x => 
                {
                    navigationService.ShowEditBookView(user, model, adminViewModel);
                },
                y => 
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });

            RemoveBookCommand = new DelegateCommand(
                x => 
                {
                    if (MessageBox.Show("Wollen Sie das Buch " + Model.BasicInfos.Titel + " wirklich Löschen?", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Delete in DB
                            _library.RemoveBook(model);

                            // Delete in View
                            var deleteBook = _adminViewModel.BookList.Where(x => x.Model.BasicInfos.Id == model.BasicInfos.Id).FirstOrDefault();
                            _adminViewModel.BookList.Remove(deleteBook);
                        }
                        catch
                        {
                            MessageBox.Show("Fehler beim buch löschen, Buch wurde nicht gelöscht");
                        }
                    }
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.BIBOTEAM;
                });
        }
    }
}
