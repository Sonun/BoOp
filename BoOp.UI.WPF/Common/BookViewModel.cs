using BoOp.DBAccessor.Models;
using BoOp.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoOp.UI.WPF.Views;
using System.Windows.Threading;
using BoOp.UI.WPF.ViewModels.ViewModelUtils;

namespace BoOp.UI.WPF.Common
{
    public class BookViewModel : ViewModel
    {
        public BuchModel Model { get; set; }
        public DelegateCommand ShowBookCommand { get; set; }

        public BookViewModel(BuchModel model, Dispatcher dispatcher, PersonModel personModel = null)
        {
            Model = model;

            ShowBookCommand = new DelegateCommand(
                x => {
                    var view = new MainWindowViewModel(dispatcher, true);
                    view.ShowBookDetailsView(personModel, model);

                    var newWindow = new MainWindow(view);
                    newWindow.Height = 800;
                    newWindow.Width = 1200;
                    newWindow.WindowState = System.Windows.WindowState.Minimized;
                    newWindow.Topmost = true;
                    newWindow.Focus();
                    newWindow.Activate();
                    newWindow.ShowActivated = true;
                    newWindow.Show();

                },
                y => { return personModel != null; });
        }

        
    }
}
