using System.Windows;
using System.Windows.Controls;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    public class MainDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoginViewModelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                LoginViewModel _ => LoginViewModelTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}