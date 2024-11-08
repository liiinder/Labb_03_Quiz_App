using System.Windows;
using System.Windows.Input;

namespace Labb_03_Quiz_App.View.Windows
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
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e) => Close();
    }
}