using Labb_3_Quiz_App.View.Windows;
using Labb_3_Quiz_App.ViewModels;
using System.Windows;

namespace Labb_3_Quiz_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Fetch fetch = new();

            fetch.ShowDialog();
        }
        private void Pack_Options(object sender, RoutedEventArgs e)
        {
            PackOptions pack = new();

            pack.ShowDialog();
        }
    }
}

// 1. Skapa en json fil i en mapp under användarens AppData\Local
// som lagrar samtliga “question packs” och laddar in dem automatiskt när
// programmet startar. Detta är alternativet jag gjort i min app.
// För att hitta mappen kan du använda:
//
// Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//
// Tänk på att programmet ska fungera även om mappen och/eller filen inte finns;
// den måste i så fall skapa upp mapp / fil.

// 2. Använd OpenFileDialog() / SaveFileDialog()
// och låt användaren spara/ladda varje “question pack” i en separat fil.