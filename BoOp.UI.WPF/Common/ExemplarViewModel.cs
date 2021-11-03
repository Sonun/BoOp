using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.Common
{
    public class ExemplarViewModel : ViewModel
    {
        public ExemplarModel Model { get; set; }
        private bool _isChecked;
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; OnPropertyChanged(); } }

        public ExemplarViewModel(ExemplarModel model)
        {
            Model = model;
        }
    }
}
