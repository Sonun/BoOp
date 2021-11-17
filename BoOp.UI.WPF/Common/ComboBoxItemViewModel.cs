using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Common
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: ComboBoxItemViewModel.cs
    //Author : Manuel Janzen
    //Erstellt am : 4/11/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : View Model für combobox items
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ComboBoxItemViewModel : ViewModel
    {
        public string Content { get; set; }

        public ComboBoxItemViewModel(string content)
        {
            Content = content;
        }
    }
}
