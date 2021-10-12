using BoOp.UI.WPF.ViewModels;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;
using System.Windows;

namespace BoOp.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public INavigationService NavigationService { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            NavigationService = new MainWindowViewModel(Dispatcher);
            DataContext = NavigationService;
        }

        public MainWindow(INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = navigationService;
        }
    }
}
