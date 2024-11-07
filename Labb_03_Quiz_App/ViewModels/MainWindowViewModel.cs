using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Models;
using Labb_03_Quiz_App.View.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigViewModel.RaisePropertyChanged("ActivePack");
                GameViewModel.RaisePropertyChanged("ActivePack");
            }
        }

        public ConfigViewModel ConfigViewModel { get; }
        private bool _inConfigMode;
        public bool InConfigMode
        {
            get => _inConfigMode;
            set
            {
                _inConfigMode = value;
                RaisePropertyChanged();
                ConfigViewModel.RaisePropertyChanged("IsActive");
            }
        }

        public GameViewModel GameViewModel { get; }
        private bool _inGameMode;
        public bool InGameMode
        {
            get => _inGameMode;
            set
            {
                _inGameMode = value;
                RaisePropertyChanged();
                GameViewModel.RaisePropertyChanged("IsActive");
                GameViewModel.StartQuiz();
            }
        }

        public string CurrentMode { get; set; }

        public DelegateCommand AddNewPackCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SwitchModeCommand { get; }
        public DelegateCommand ExitWindowCommand { get; }
        public DelegateCommand ImportQuestionsCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

        public MainWindowViewModel()
        {
            ConfigViewModel = new ConfigViewModel(this);
            GameViewModel = new GameViewModel(this);

            InConfigMode = true;
            CurrentMode = "ConfigMode";

            ActivePack = null;
            Packs = new ObservableCollection<QuestionPackViewModel>();

            AddNewPackCommand = new DelegateCommand(AddNewPack);
            SelectPackCommand = new DelegateCommand(SelectPack);
            DeletePackCommand = new DelegateCommand(DeletePack);
            SwitchModeCommand = new DelegateCommand(SwitchMode);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
            FullScreenCommand = new DelegateCommand(FullScreen);
            ImportQuestionsCommand = new DelegateCommand(ImportQuestions);
        }

        private void FullScreen(object obj)
        {
            var mainWindow = Application.Current.MainWindow;

            if (mainWindow.WindowState == WindowState.Normal)
            {
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.WindowState = WindowState.Normal;
            }
        }

        public async Task LoadPacks()
        {
            Directory.CreateDirectory(pathToFolder);
            if (!File.Exists(pathToFile)) File.WriteAllText(pathToFile, "[]");

            string json = await File.ReadAllTextAsync(pathToFile);

            var loadedPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);
            foreach (QuestionPackViewModel pack in loadedPacks) Packs.Add(pack);
            if (ActivePack is null && Packs.Count > 0) ActivePack = Packs[^1];
        }

        private void SwitchMode(object obj)
        {
            if (obj is string button)
            {
                if (button != CurrentMode)
                {
                    CurrentMode = button;
                    InGameMode = !InGameMode;
                    InConfigMode = !InConfigMode;
                }
            }
        }

        private void DeletePack(object obj)
        {
            Packs.Remove(ActivePack);
            ActivePack = (Packs.Count > 0) ? Packs[^1] : null;
        }

        private void SelectPack(object obj)
        {
            if (obj is QuestionPackViewModel q) ActivePack = q;
        }

        private void AddNewPack(object obj)
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack());
            Packs.Add(ActivePack);
            new PackOptions().ShowDialog();
        }

        private static string pathToFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb_03_Quiz_App");
        private static string pathToFile = Path.Combine(pathToFolder, "QuestionPacks.json");

        public void ExitWindow(object obj)
        {
            string jsonString = JsonSerializer.Serialize(Packs);
            File.WriteAllText(pathToFile, jsonString);
            Environment.Exit(0);
        }

        public void ImportQuestions(object obj)
        {
            string json = File.ReadAllText(pathToFolder + "\\Hardcoded_API_Response.json");
            var importedQuestions = JsonSerializer.Deserialize<List<OpenTDb>>(json);
            Debug.WriteLine(json[0]);
            Debug.WriteLine(json[0]);
        }
    }
}
