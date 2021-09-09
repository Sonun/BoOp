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
  /// Interaktionslogik für BookView.xaml
  /// </summary>
  public partial class BookView : UserControl
  {
    public BookView()
    {
      InitializeComponent();
    }

    private void TextBox_TextChanged()
    {
    }

    private void Button_ClickLoginPage(object sender, RoutedEventArgs e)
    {
      var win = new LoginView();
      win.Width = 280;
      win.Height = 380;
      win.Show();
    }
  }
}