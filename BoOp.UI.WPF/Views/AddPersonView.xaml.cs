using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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