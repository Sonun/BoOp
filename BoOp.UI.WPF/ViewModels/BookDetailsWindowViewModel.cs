using BoOp.UI.WPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class BookDetailsWindowViewModel : ViewModel
    {
        private BookViewModel _bookViewModel;

        public BookDetailsWindowViewModel(BookViewModel bookViewModel)
        {
            _bookViewModel = bookViewModel;
        }
    }
}
