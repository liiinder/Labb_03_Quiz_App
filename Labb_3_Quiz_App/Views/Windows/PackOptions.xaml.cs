using System.Windows;

namespace Labb_3_Quiz_App.View.Windows
{
    /// <summary>
    /// Interaction logic for PackOptions.xaml
    /// </summary>
    public partial class PackOptions : Window
    {
        public PackOptions()
        {
            InitializeComponent();

            DataContext = (App.Current.MainWindow as MainWindow)?.DataContext;
        }
    }
}
