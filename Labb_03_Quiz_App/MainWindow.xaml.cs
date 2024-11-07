using Labb_03_Quiz_App.View.Windows;
using Labb_03_Quiz_App.ViewModels;
using System.Windows;
using static System.Windows.Forms.AxHost;

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
            await mainWindowVM.LoadPacks();
        }
    }
}