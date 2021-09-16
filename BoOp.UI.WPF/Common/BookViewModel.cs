using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.Common
{
    public class BookViewModel : ViewModel
    {
        public BuchModel Model { get; set; }

        public BookViewModel(BuchModel model)
        {
            Model = model;
        }
    }
}
