using Labb_03_Quiz_App.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Labb_03_Quiz_App.Dialogs
{
    /// <summary>
    /// Interaction logic for Fetch.xaml
    /// </summary>
    public partial class FetchQuestionsDialog : Window
    {
        public FetchQuestionsDialog()
        {
            InitializeComponent();

            DataContext = (App.Current.MainWindow as MainWindow)?.DataContext;
            Owner = App.Current.MainWindow;
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e) => Close();

        private async void Import_Categories(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowVM)
            {
                mainWindowVM.HasImportedCategories = await mainWindowVM.OpenTDbAPI.ImportCategories();
            }
        }
        private async void Import_Questions(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel mainWindowVM && mainWindowVM.ActivePack is not null)
            {
                bool result = await mainWindowVM.OpenTDbAPI.ImportQuestions(mainWindowVM.ActivePack);

                if (result) Close();
            }
        }
    }
}