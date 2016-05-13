using System.Windows;
using LogExample.ViewModel;

namespace LogExample.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            Closing += (o, e) => ((MainViewModel)DataContext).Dispose();
        }
    }
}
