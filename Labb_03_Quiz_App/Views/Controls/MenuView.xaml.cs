using Labb_03_Quiz_App.View.Windows;
using System.Windows;
using System.Windows.Controls;

namespace Labb_03_Quiz_App.View.Controls
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void Pack_Options(object sender, RoutedEventArgs e)
        {
            PackOptions pack = new();

            pack.ShowDialog();
        }
    }
}
