using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.Common
{
    public class RatingViewModel : ViewModel
    {
        public int Rating { get; set; }

        public RatingViewModel(int rating)
        {
            Rating = rating;
        }
    }
}
