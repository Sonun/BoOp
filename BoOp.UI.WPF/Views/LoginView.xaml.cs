using System.Windows;
using System.Windows.Controls;

namespace BoOp.UI.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class LoginView : UserControl
      {
            public LoginView()
            {
              InitializeComponent(); 
            }

            private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
            {
                if (DataContext != null)
                { ((dynamic)DataContext).Password = ((PasswordBox)sender).Password; }
            }

        }
}