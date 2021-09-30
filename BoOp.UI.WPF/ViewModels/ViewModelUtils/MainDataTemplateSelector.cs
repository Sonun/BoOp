using System.Windows;
using System.Windows.Controls;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    public class MainDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ScanUserViewModelTemplate { get; set; }
        public DataTemplate LoginViewModelTemplate { get; set; }
        public DataTemplate LibraryViewModelTemplate { get; set; }
        public DataTemplate AdminViewModelTemplate { get; set; }
        public DataTemplate AddPersonViewModelTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ScanUserViewModel _ => ScanUserViewModelTemplate,
                LoginViewModel _ => LoginViewModelTemplate,
                LibraryViewModel _ => LibraryViewModelTemplate,
                AdminViewModel _ => AdminViewModelTemplate,
                AddPersonViewModel _ => AddPersonViewModelTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}