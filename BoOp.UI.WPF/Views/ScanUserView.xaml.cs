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
    /// Interaktionslogik für ScanUser.xaml
    /// </summary>
    public partial class ScanUserView : UserControl
    {
        public ScanUserView()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => SetFocusScanBox());

        }
        public void SetFocusScanBox()
        {
            Dispatcher.Invoke(() => ScanBox.Focus());
        }
        
        
    }
}
