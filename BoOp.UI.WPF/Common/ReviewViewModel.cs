using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.UI.WPF.Views;
using System.Windows.Threading;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;

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
