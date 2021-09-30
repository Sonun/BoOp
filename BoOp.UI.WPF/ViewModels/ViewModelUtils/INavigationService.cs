using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    public interface INavigationService
    {
        void ShowLoginView();
        void ShowScanUserView();
        void ShowLibraryView();
        void ShowAdminView();
    }
}
