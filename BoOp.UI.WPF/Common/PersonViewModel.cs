using System.Windows;
using System.Linq;
using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: PersonViewModel.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 26/10/2021
    //Bearbeitet von : Manuel Janzen, Dominik von Michalkowsky
    //Beschreibung : View Model für personen
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class PersonViewModel : ViewModel
    {
        private readonly ILibrary _library;
        private readonly AdminViewModel _adminViewModel;

        public PersonModel Model { get; set; }

        public DelegateCommand EditUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }
        public DelegateCommand ShowUserCommand { get; set; }
        public DelegateCommand DeleteFromListCommand { get; set; }
        public DelegateCommand AddPrintListCommand { get; set; }

        public PersonViewModel(PersonModel personModel, INavigationService navigationService, ILibrary library, PersonModel editor, AdminViewModel adminViewModel)
        {
            Model = personModel;
            _library = library;
            _adminViewModel = adminViewModel;

            EditUserCommand = new DelegateCommand(
                    x =>
                    {
                        navigationService.ShowEditUserView(editor, Model);
                    },
                    y =>
                    {
                        // bearbeiten von benutzer "maskenpflicht" nicht möglich, ansonsten nur untergestellte
                        return Model.AusweisID.ToLower().Equals("ricksanchez") ? false : editor.Rechte >= Rechtelevel.BIBOTEAM;
                    });
            // Remove user from db --> used in AdminViewModel
            RemoveUserCommand = new DelegateCommand(
                x =>
                {
                    if (MessageBox.Show("Wollen Sie den Benutzer " + Model.Vorname + " " + Model.Nachname + " wirklich Löschen?", "Löschen?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Delete in DB
                            _library.RemoveUser(Model);

                            // Delete in View
                            var deletePerson = _adminViewModel.UserList.Where(x => x.Model.Id == Model.Id).FirstOrDefault();
                            _adminViewModel.UserList.Remove(deletePerson);
                        }
                        catch
                        {
                            MessageBox.Show("Fehler beim Benutzer löschen, Benutzer wurde nicht gelöscht");
                        }
                    }
                    },
                y =>
                {
                    // löschen von benutzer "maskenpflicht" nicht möglich, ansonsten nur untergestellte
                    return Model.AusweisID.ToLower().Equals("ricksanchez") ? false : editor.Rechte >= Rechtelevel.BIBOTEAM;
                });

            DeleteFromListCommand = new DelegateCommand(
                x =>
                {
                    var deleteModel = AdminViewModel.StaticUserIDPrintList.Single(x => x.Model.AusweisID == Model.AusweisID);
                    AdminViewModel.StaticUserIDPrintList.Remove(deleteModel);
                });

            ShowUserCommand = new DelegateCommand(x =>
            {
                var lendedBooks = adminViewModel.GetLendedBooksFromUser(personModel);
                navigationService.ShowUserView(lendedBooks, personModel, editor);
            },
            y =>
            {
                // details von benutzer "maskenpflicht" nicht möglich
                return !Model.AusweisID.ToLower().Equals("ricksanchez");
            });

            AddPrintListCommand = new DelegateCommand(x =>
            {
                AdminViewModel.StaticUserIDPrintList.Add(this);
            },
            y =>
            {
                // drucken von benutzer "maskenpflicht" nicht möglich
                return !Model.AusweisID.ToLower().Equals("ricksanchez");
            });
        }
    }
}
