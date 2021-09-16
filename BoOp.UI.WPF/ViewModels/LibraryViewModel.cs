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

namespace BoOp.UI.WPF.ViewModels
{
  public class LibraryViewModel : ViewModel
  {
    private INavigationService _navigationService;
    private ObservableCollection<BookViewModel> _bookList;

    public DelegateCommand OpenLoginView { get; set; }

    public ObservableCollection<BookViewModel> BookList { 
            get 
            { 
                return _bookList; 
            } 
            set
            {
                _bookList = value;
                OnPropertyChanged();
            }}

    public LibraryViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        OpenLoginView = new DelegateCommand(
            x =>
            {
              _navigationService.ShowLoginView();
            });

        Library l = new Library();
        BookList = new ObservableCollection<BookViewModel>();

        foreach (var book in l.GetAllBooks())
        {
           BookList.Add(new BookViewModel(book));
        }
    }
  }
}
