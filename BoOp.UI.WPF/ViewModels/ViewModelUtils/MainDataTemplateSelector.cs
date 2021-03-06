using System.Windows;
using System.Windows.Controls;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: MainDataTemplateSelector.cs
    //Author : Dominik von Michalkowsky
    //Erstellt am : 2/09/2021
    //Bearbeitet von : Dominik von Michalkowsky, Manuel Janzen
    //Beschreibung : klasse die zur auswahl von viewmodels für die jeweiligen views benötigt wird
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class MainDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ScanUserViewModelTemplate { get; set; }
        public DataTemplate LoginViewModelTemplate { get; set; }
        public DataTemplate LibraryViewModelTemplate { get; set; }
        public DataTemplate AdminViewModelTemplate { get; set; }
        public DataTemplate AddPersonViewModelTemplate { get; set; }
        public DataTemplate AddBookViewModelTemplate { get; set; }
        public DataTemplate EditPersonViewModelTemplate { get; set; }
        public DataTemplate LendBookViewModelTemplate { get; set; }
        public DataTemplate ReturnBookViewModelTemplate { get; set; }
        public DataTemplate EditBookViewModelTemplate { get; set; }
        public DataTemplate ShowUserViewModelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ScanUserViewModel _ => ScanUserViewModelTemplate,
                LoginViewModel _ => LoginViewModelTemplate,
                LibraryViewModel _ => LibraryViewModelTemplate,
                AdminViewModel _ => AdminViewModelTemplate,
                AddPersonViewModel _ => AddPersonViewModelTemplate,
                AddBookViewModel _ => AddBookViewModelTemplate,
                EditPersonViewModel _ => EditPersonViewModelTemplate,
                LendBookViewModel _ => LendBookViewModelTemplate,
                ReturnBookViewModel _ => ReturnBookViewModelTemplate,
                EditBookViewModel _ => EditBookViewModelTemplate,
                ShowUserViewModel _ => ShowUserViewModelTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}