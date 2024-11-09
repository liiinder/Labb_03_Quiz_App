using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Dialogs;
using Labb_03_Quiz_App.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
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
        private bool _hasImportedCategories;
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
                RaisePropertyChanged("CanImportOrPlay");
            }
        }
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ConfigViewModel ConfigViewModel { get; }
        public GameViewModel GameViewModel { get; }
        public OpenTDb OpenTDbApi { get; set; }
        public string CurrentMode { get; set; }
        public bool InConfigMode
        {
            get => _inConfigMode;
            set
            {
                _inConfigMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanImportOrPlay");
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
                if (ActivePack is not null) GameViewModel.StartQuizCommand.Execute(null);
            }
        }

        //TODO: Clean up and add options to disable menu options if conditions is not met.
        // like only delete packs if there is a pack to delete
        // delete question menu option if any is selected

        public bool CanImportOrPlay { get => (InConfigMode && ActivePack is not null); }
        public bool HasImportedCategories
        {
            get => _hasImportedCategories;
            set
            {
                _hasImportedCategories = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand AddNewPackCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand OpenImportQuestionsDialogCommand { get; }
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
            OpenTDbApi = new OpenTDb(Categories[0]);

            AddNewPackCommand = new DelegateCommand(AddNewPack, ConfigViewModel.CanModifyPacks);
            SelectPackCommand = new DelegateCommand(SelectPack, ConfigViewModel.CanModifyPacks);
            DeletePackCommand = new DelegateCommand(DeletePack, ConfigViewModel.CanModifyPacks);
            OpenImportQuestionsDialogCommand = new DelegateCommand(OpenImportQuestionDialog, ConfigViewModel.CanModifyPacks);
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

                var jsonDeserialized = JsonSerializer.Deserialize<Categories>(response);
                if (jsonDeserialized is not null)
                {
                    foreach (Category cat in jsonDeserialized.ListOfCategories) Categories.Add(cat);
                }
                HasImportedCategories = true;
            }
            catch
            {
            }

            //TODO: Add some kind of feedback to the user under the "Import Category" button that this is importing...
            // Maybe a small loading animation?
            // And then the correct catch message if it doesnt work.
        }

        public async Task ImportQuestions()
        {
            string response = string.Empty;
            try
            {
                response = await client.GetStringAsync(OpenTDbApi.Url);

                var deserialized = JsonSerializer.Deserialize<OpenTDb>(response);
                if (deserialized is not null && deserialized.Response == 0)
                {
                    foreach (Question q in deserialized.ListOfQuestions)
                    {
                        Question temp = new();
                        temp.Query = WebUtility.HtmlDecode(q.Query);
                        temp.CorrectAnswer = WebUtility.HtmlDecode(q.CorrectAnswer);
                        temp.IncorrectAnswers = q.IncorrectAnswers.Select(item => WebUtility.HtmlDecode(item)).ToArray();
                        ActivePack?.Questions.Add(temp);
                    }
                    ConfigViewModel.SelectedQuestion = ActivePack?.Questions[^1];
                }
            }
            catch
            {
            }

            //TODO: Add response feedback to the user if the import fails
            // If it fails because there isnt enough questions, set slider to the amount that exists

            //TODO: Look into making the project/code more "SOLID"
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
        private void OpenImportQuestionDialog(object obj)
        {
            new FetchQuestionsDialog().ShowDialog();
        }
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
