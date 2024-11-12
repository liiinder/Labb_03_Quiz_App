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
            mainWindowVM = new MainWindowViewModel();
            DataContext = mainWindowVM;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindowVM.ExitWindowCommand.Execute(null);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //await mainWindowVM.LoadPacks();
            JsonHandler json = new();
            await json.LoadPacks(mainWindowVM);

            mainWindowVM.HasImportedCategories = await mainWindowVM.OpenTDbAPI.ImportCategories();
            if (mainWindowVM.ActivePack is not null) await mainWindowVM.OpenTDbAPI.GetToken(mainWindowVM.ActivePack);
        }
    }
}