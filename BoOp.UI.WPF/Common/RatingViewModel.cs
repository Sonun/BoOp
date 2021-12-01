using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: RatingViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 27/10/2021
    //Bearbeitet von : Manuel Janzen, Dominik von Michalkowsky
    //Beschreibung : View Model für bewertungen
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class RatingViewModel : ViewModel
    {
        public double Rating { get; set; }

        public RatingViewModel(double rating)
        {
            Rating = rating;
        }
    }
}
