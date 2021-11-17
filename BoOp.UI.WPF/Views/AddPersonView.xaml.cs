using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace BoOp.UI.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für AddPersonView.xaml
    /// </summary>
    public partial class AddPersonView : UserControl
    {
        public AddPersonView()
        {
            InitializeComponent();

            SolidColorBrush required = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1B1C"));

            if (String.IsNullOrEmpty(VornameTextbox.Text))
            {
                VornameLabel.Foreground = required;
            }
            else
            {
                VornameLabel.Foreground = Brushes.Black;
            }
            if (String.IsNullOrEmpty(NachnameTextbox.Text))
            {
                NachnameLabel.Foreground = required;
            }
            else
            {
                NachnameLabel.Foreground = Brushes.Black;
            }
            
        }
    }
}