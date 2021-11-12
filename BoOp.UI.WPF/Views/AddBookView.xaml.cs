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
    /// Interaktionslogik für AddBookView.xaml
    /// </summary>
    public partial class AddBookView : UserControl
    {
        public AddBookView()
        {
            InitializeComponent();

            SolidColorBrush required = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1B1C"));

            if (String.IsNullOrEmpty(TitelTextbox.Text))
            {
                TitelLabel.Foreground = required;
            }
            else
            {
                TitelLabel.Foreground = Brushes.Black;
            }

            if (String.IsNullOrEmpty(AuthorTextbox.Text))
            {
                AuthorLabel.Foreground = required;
            }
            else
            {
                AuthorLabel.Foreground = Brushes.Black;
            }

            if (String.IsNullOrEmpty(VerlagTextbox.Text))
            {
                VerlagLabel.Foreground = required;
            }
            else
            {
                VerlagLabel.Foreground = Brushes.Black;
            }

            if (String.IsNullOrEmpty(SchlagwoerterTextbox.Text))
            {
                SchlagwoerterLabel.Foreground = required;
            }
            else
            {
                SchlagwoerterLabel.Foreground = Brushes.Black;
            }
        }
    }
}