using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ReviewViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 26/10/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : View Model für reviews
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ReviewViewModel : ViewModel
    {
        public RezensionModel RezensionModel {get; set;}

        public ReviewViewModel(RezensionModel model)
        {
            RezensionModel = model;
        }
    }
}
