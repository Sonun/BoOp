using BoOp.DBAccessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    public interface INavigationService
    {
        void ShowLoginView(PersonModel user);
        void ShowScanUserView();
        void ShowLibraryView(PersonModel user);
        void ShowAdminView(PersonModel user);
        void ShowAddPersonView(PersonModel user);
        void ShowAddBookView(PersonModel user);
        void ShowEditUserView(PersonModel user);
    }
}
