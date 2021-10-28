using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using System.Collections.ObjectModel;

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
        void ShowLendBookView(PersonModel user);
        void ShowReturnLendBookView(PersonModel user);
        void ShowRemoveBookView(PersonModel user, BuchModel book);
        void ShowEditBookView(PersonModel user, BuchModel book);
    }
}
