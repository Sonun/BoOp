using System;
using System.Windows.Controls;
using System.Windows.Media;

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

            if (String.IsNullOrEmpty(ISBNTextBox.Text))
            {
                ISBNLabel.Foreground = required;
            }
            else
            {
                ISBNLabel.Foreground = Brushes.Black;
            }
        }
    }
}