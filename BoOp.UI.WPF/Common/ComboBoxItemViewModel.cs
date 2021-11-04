using BoOp.UI.WPF.ViewModels;

namespace BoOp.UI.WPF.Common
{
    public class ComboBoxItemViewModel : ViewModel
    {
        public string Content { get; set; }

        public ComboBoxItemViewModel(string content)
        {
            Content = content;
        }
    }
}
