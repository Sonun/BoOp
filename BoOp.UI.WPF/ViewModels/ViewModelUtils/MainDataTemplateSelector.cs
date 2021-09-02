﻿using System.Windows;
using System.Windows.Controls;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.ViewModels.ViewModelUtils
{
    public class MainDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ScanUserViewModelTemplate { get; set; }
        public DataTemplate LoginViewModelTemplate { get; set; }
        public DataTemplate BookViewModelTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                ScanUserViewModel _ => ScanUserViewModelTemplate,
                LoginViewModel _ => LoginViewModelTemplate,
                BookViewModel _ => BookViewModelTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}