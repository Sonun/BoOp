using System.Threading.Tasks;
using System.Windows.Controls;

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
