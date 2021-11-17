using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: RatingViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 27/10/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : View Model für personen
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class RatingViewModel : ViewModel
    {
        public int Rating { get; set; }

        public RatingViewModel(int rating)
        {
            Rating = rating;
        }
    }
}
