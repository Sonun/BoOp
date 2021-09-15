using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
  public class BookViewModel : ViewModel
  {
    private INavigationService _navigationService;
    public DelegateCommand OpenLoginView { get; set; }
    public BookViewModel(INavigationService navigationService)
    {
      _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
      OpenLoginView = new DelegateCommand(
        x =>
        {
          _navigationService.ShowLoginView();
        });
    }
  }
}
