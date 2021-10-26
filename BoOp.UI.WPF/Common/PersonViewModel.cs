using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Common
{
    public class PersonViewModel : ViewModel
    {
        public PersonModel Model;

        public PersonViewModel(PersonModel personModel)
        {
            Model = personModel;
        }
    }
}
