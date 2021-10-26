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

namespace BoOp.UI.WPF.Common
{
    public class BookViewModel : ViewModel
    {
        public BuchModel Model { get; set; }
        private LibraryViewModel _libraryViewModel;
        public DelegateCommand ShowBookCommand { get; set; }
        public DelegateCommand EditBookCommand { get; set; }
        public DelegateCommand RemoveBookCommand { get; set; }

        public BookViewModel(BuchModel model, INavigationService navigationService, LibraryViewModel libraryViewModel, PersonModel user)
        {
            Model = model;
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
                x => {
                    navigationService.ShowRemoveBookView(user, model);
                },
                y =>
                {
                    return user.Rechte >= Rechtelevel.ADMIN;
                });
        }
    }
}
