using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für BookDetailsWindow.xaml
    /// </summary>
    public partial class BookDetailsWindow : Window
    {
        private bool timerflag = false;

        public BookDetailsWindow(Common.BookViewModel bookViewModel)
        {
            InitializeComponent();
            DataContext = new BookDetailsWindowViewModel(bookViewModel);

            var timer = new Timer(x => { Dispatcher.Invoke(dothis);  }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10)); 
        }

        private void dothis()
        {
            if (timerflag)
                this.Close();
            else
                timerflag = true;
        }
    }
}
