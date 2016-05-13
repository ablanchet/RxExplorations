using System.Windows;
using LogExample.ViewModel;

namespace LogExample.View
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
        public SubWindow()
        {
            InitializeComponent();
            DataContext = new SubWindowViewModel();

            Closing += (o, e) => ((SubWindowViewModel)DataContext).Dispose();
        }
    }
}
