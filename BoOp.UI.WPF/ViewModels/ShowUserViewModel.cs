using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.ViewModels
{
    public class ShowUserViewModel : ViewModel
    {
        public PersonModel PersonModel { get; set; }
        public ObservableCollection<ExemplarViewModel> LendedBooks { get; set; }

        public ShowUserViewModel(PersonModel personModel, List<ExemplarViewModel> exemplarViewModels)
        {
            LendedBooks = new ObservableCollection<ExemplarViewModel>();
            PersonModel = personModel;
            exemplarViewModels.ForEach(x => LendedBooks.Add(x));
        }
    }
}
