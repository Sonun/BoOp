using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: INavigationService.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 1/09/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : interface zur implementierung der navigations schnittstelle
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public interface INavigationService
    {
        void ShowLoginView(PersonModel user);
        void ShowScanUserView();
        void ShowLibraryView(PersonModel user);
        void ShowAdminView(PersonModel user);
        void ShowAddPersonView(PersonModel user);
        void ShowAddBookView(PersonModel user);
        void ShowEditUserView(PersonModel editor, PersonModel userToChange);
        void ShowLendBookView(PersonModel user);
        void ShowReturnLendBookView(PersonModel user);
        void ShowEditBookView(PersonModel user, BuchModel book, AdminViewModel adminViewModel);
        void ShowUserView(List<ExemplarViewModel> exemplare, PersonModel user, PersonModel loggedinUser);
    }
}
