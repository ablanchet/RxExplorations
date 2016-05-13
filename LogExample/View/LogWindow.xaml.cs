using System.Windows;
using LogExample.ViewModel;

namespace LogExample.View
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            DataContext = new LogViewModel();
        }
    }
}
