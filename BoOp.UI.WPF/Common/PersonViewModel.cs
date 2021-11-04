using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.Business;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.Common
{
    public class PersonViewModel : ViewModel
    {
        private readonly ILibrary _library;
        private readonly AdminViewModel _adminViewModel;

        public PersonModel Model { get; set; }

        public DelegateCommand EditUserCommand { get; set; }
        public DelegateCommand RemoveUserCommand { get; set; }

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
                        return editor.Rechte >= Rechtelevel.BIBOTEAM;
                    });
            RemoveUserCommand = new DelegateCommand(
                x =>
                {
                    // Delete in DB
                    _library.RemoveUser(Model);

                    // Delete in View
                    var deletePerson = _adminViewModel.UserList.Where(x => x.Model.Id == Model.Id).FirstOrDefault();
                    _adminViewModel.UserList.Remove(deletePerson);
                },
                y =>
                {
                    return editor.Rechte >= Rechtelevel.ADMIN;
                });
        }
    }
}
