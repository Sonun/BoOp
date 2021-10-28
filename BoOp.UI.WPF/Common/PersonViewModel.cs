using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.Common
{
    public class PersonViewModel : ViewModel
    {
        public PersonModel Model { get; set; }

        public DelegateCommand EditUserCommand { get; set; }

        public PersonViewModel(PersonModel personModel, INavigationService navigationService, PersonModel editor)
        {
            Model = personModel;


            EditUserCommand = new DelegateCommand(
                    x =>
                    {
                        navigationService.ShowEditUserView(editor, Model);
                    },
                    y =>
                    {
                        return editor.Rechte >= Rechtelevel.BIBOTEAM;
                    });
        }
    }
}
