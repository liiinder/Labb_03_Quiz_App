using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Dialogs;
using Labb_03_Quiz_App.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://opentdb.com/") };

        private QuestionPackViewModel? _activePack;
        private bool _inConfigMode;
        private bool _inGameMode;
        private bool _hasInternetConnection;
        private static string pathToFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb_03_Quiz_App");
        private static string pathToFile = Path.Combine(pathToFolder, "QuestionPacks.json");

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
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ConfigViewModel ConfigViewModel { get; }
        public GameViewModel GameViewModel { get; }
        public string CurrentMode { get; set; }
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
        public bool InGameMode
        {
            get => _inGameMode;
            set
            {
                _inGameMode = value;
                RaisePropertyChanged();
                GameViewModel.RaisePropertyChanged("IsActive");
                GameViewModel.StartQuizCommand.Execute(null);
            }
        }
        public bool HasInternetConnection
        {
            get => _hasInternetConnection;
            set
            {
                _hasInternetConnection = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand AddNewPackCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand ImportQuestionsCommand { get; }
        public DelegateCommand ExitWindowCommand { get; }
        public DelegateCommand SwitchModeCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

        public MainWindowViewModel()
        {
            ConfigViewModel = new ConfigViewModel(this);
            GameViewModel = new GameViewModel(this);

            InConfigMode = true;
            CurrentMode = "ConfigMode";

            ActivePack = null;
            Packs = new ObservableCollection<QuestionPackViewModel>();
            Categories = new ObservableCollection<Category>();
            Categories.Add(new Category(0, "Any"));

            AddNewPackCommand = new DelegateCommand(AddNewPack, ConfigViewModel.CanModifyPacks);
            SelectPackCommand = new DelegateCommand(SelectPack, ConfigViewModel.CanModifyPacks);
            DeletePackCommand = new DelegateCommand(DeletePack, ConfigViewModel.CanModifyPacks);
            ImportQuestionsCommand = new DelegateCommand(ImportQuestions, CanImportQuestions);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
            SwitchModeCommand = new DelegateCommand(SwitchMode);
            FullScreenCommand = new DelegateCommand(FullScreen);
        }

        public async Task ImportCategories()
        {
            string response = string.Empty;
            try
            {
                response = await client.GetStringAsync("api_category.php");
            }
            finally
            {
                var jsonDeserialized = JsonSerializer.Deserialize<Categories>(response);
                if (jsonDeserialized is not null)
                {
                    foreach (Category cat in jsonDeserialized.ListOfCategories) Categories.Add(cat);
                }
                HasInternetConnection = true;
            }
            //TODO: fix so that you can use the import questions button in the menu
            // if HasInternetConnection is false, and then run this method again.
            // if it fails, pop up a message that the user doesn't have Internet connection.
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
        private void AddNewPack(object obj)
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack());
            Packs.Add(ActivePack);
            new PackOptionsDialog().ShowDialog();
        }
        private void SelectPack(object obj)
        {
            if (obj is QuestionPackViewModel q) ActivePack = q;
        }
        private void DeletePack(object obj)
        {
            Packs.Remove(ActivePack);
            ActivePack = (Packs.Count > 0) ? Packs[^1] : null;
        }
        private bool CanImportQuestions(object? arg) => (HasInternetConnection && InConfigMode && ActivePack is not null);
        private void ImportQuestions(object obj) => new FetchQuestionsDialog().ShowDialog();
        //TODO: Implement ImportQuestions...

        // url = https://opentdb.com/api.php?amount=10&category=17&difficulty=easy&type=multiple
        // string difficultyUrl = (choosendiff.. == Any) ? "" : "&difficulty={choosendiff..}";
        // $"amount={sliderValue..}{choosenCategory.Url..}{difficultyUrl}&type=multiple";

        //TODO: Add functionality so if you add questions from OpenTDb that it doesn't add duplicates

        private void ExitWindow(object obj)
        {
            string jsonString = JsonSerializer.Serialize(Packs);
            File.WriteAllText(pathToFile, jsonString);
            Environment.Exit(0);
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
        private void FullScreen(object obj)
        {
            var main = Application.Current.MainWindow;

            if (main.WindowState == WindowState.Normal)
            {
                main.WindowStyle = WindowStyle.None;
                main.WindowState = WindowState.Maximized;
            }
            else
            {
                main.WindowStyle = WindowStyle.SingleBorderWindow;
                main.WindowState = WindowState.Normal;
            }
        }
    }
}
