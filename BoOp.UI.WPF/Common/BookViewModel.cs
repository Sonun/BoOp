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
        public DelegateCommand ShowBookCommand { get; set; }
        private LibraryViewModel _libraryViewModel;

        public BookViewModel(BuchModel model, INavigationService navigationService, LibraryViewModel libraryViewModel, PersonModel personModel = null)
        {
            Model = model;
            _libraryViewModel = libraryViewModel;

            ShowBookCommand = new DelegateCommand(
                x => {
                    _libraryViewModel.BookDetailsViewModel = new BookDetailsViewModel(navigationService, personModel, model, libraryViewModel);
                },
                y => { return personModel != null; });
        }

        
    }
}
