using Labb_03_Quiz_App.Data;
using Labb_03_Quiz_App.ViewModels;
using System.Windows;

namespace Labb_03_Quiz_App
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel mainWindowVM { get; }
        public MainWindow()
        {
            InitializeComponent();
            //TODO: Look into dependency injection / interface for importer and send it into the constructor instead of hardcoding/creating OpenTDbAPI in MainWindowViewModel construction.
            mainWindowVM = new MainWindowViewModel();
            DataContext = mainWindowVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindowVM.ExitWindowCommand.Execute(null);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await new JsonHandler().LoadPacks(mainWindowVM);

            mainWindowVM.HasImportedCategories = await mainWindowVM.OpenTDbAPI.ImportCategories();
            if (mainWindowVM.ActivePack is not null) await mainWindowVM.OpenTDbAPI.GetToken(mainWindowVM.ActivePack);
        }
    }
}