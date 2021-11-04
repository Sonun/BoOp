using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.UI.WPF.Views;
using System.Windows.Threading;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;
using BoOp.Business;

namespace BoOp.UI.WPF.Common
{
    public class BookViewModel : ViewModel
    {
        public BuchModel Model { get; set; }

        private readonly ILibrary _library;
        private LibraryViewModel _libraryViewModel;
        public DelegateCommand ShowBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }

        public BookViewModel(BuchModel model, INavigationService navigationService, ILibrary library, LibraryViewModel libraryViewModel, PersonModel user)
        {
            Model = model;
            _library = library;
            _libraryViewModel = libraryViewModel;

            ShowBookCommand = new DelegateCommand(
                x => {
                    _libraryViewModel.BookDetailsViewModel = new BookDetailsViewModel(navigationService, user, model, libraryViewModel);
                },
                y => { return user != null; });

            EditBookCommand = new DelegateCommand(
                x => {
                    navigationService.ShowEditBookView(user, model);
                },
                y => { return user != null; });

            RemoveBookCommand = new DelegateCommand(
                x => 
                {
                    //ToDo: Update View
                    _library.RemoveBook(model);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.ADMIN;
                });
        }
    }
}
