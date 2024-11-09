using System.Windows;
using System.Windows.Input;

namespace Labb_03_Quiz_App.Dialogs
{
    /// <summary>
    /// Interaction logic for PackOptions.xaml
    /// </summary>
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();

            DataContext = (App.Current.MainWindow as MainWindow)?.DataContext;
            Owner = App.Current.MainWindow;
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e) => Close();
    }
}