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
        public string RatingPic
        {
            get
            {
                switch (RezensionModel.BasicInfos.Sterne)
                {
                    case (1):
                        return "/Assets/Butterflies/1butterflies.png";
                    case (< 2):
                        return "/Assets/Butterflies/1.5butterflies.png";
                    case (< 2.5):
                        return "/Assets/Butterflies/2butterflies.png";
                    case (< 3):
                        return "/Assets/Butterflies/2.5butterflies.png";
                    case (< 3.5):
                        return "/Assets/Butterflies/3butterflies.png";
                    case (< 4):
                        return "/Assets/Butterflies/3.5butterflies.png";
                    case (< 4.5):
                        return "/Assets/Butterflies/4butterflies.png";
                    case (< 5):
                        return "/Assets/Butterflies/4.5butterflies.png";
                    case (< 5.5):
                        return "/Assets/Butterflies/5butterflies.png";
                    default:
                        return "/Assets/Butterflies/0butterflies.png";
                }
            }
        }

        public ReviewViewModel(RezensionModel model)
        {
            RezensionModel = model;
        }
    }
}
