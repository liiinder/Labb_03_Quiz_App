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
        public OpenTDb OpenTDbAPI { get; set; }
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
        // Give the "CanImportOrPlay" a better name as it apparently is used for almost all config things.

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
            OpenTDbAPI = new OpenTDb(Categories[0]);

            AddNewPackCommand = new DelegateCommand(AddNewPack);
            SelectPackCommand = new DelegateCommand(SelectPack);
            DeletePackCommand = new DelegateCommand(DeletePack);
            OpenImportQuestionsDialogCommand = new DelegateCommand(OpenImportQuestionDialog);
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

            //TODO: Look into making the project/code more "SOLID"
            //
            // https://www.reddit.com/r/SwiftUI/comments/197q25y/where_do_you_put_your_api_functions_in_mvvm_in/
            //
            //TODO: Move API related things (models/methods) to a service folder/file

            // Response Codes
            // --------------
            //Dictionary<int, string> ResponseMessageList = new Dictionary<int, string>()
            //{
            //    { 0, "Success Returned results successfully." },
            //    { 1, "No Results Could not return results.The API doesn't have enough questions for your query. (Ex. Asking for 50 Questions in a Category that only has 20." },
            //    { 2, "Invalid Parameter Contains an invalid parameter. Arguements passed in aren't valid. (Ex. Amount = Five)" },
            //    { 3, "Token Not Found Session Token does not exist." },
            //    { 4, "Token Empty Session Token has returned all possible questions for the specified query.Resetting the Token is necessary." },
            //    { 5, "Rate Limit Too many requests have occurred.Each IP can only access the API once every 5 seconds." }
            //};

            //TODO If Response = 1 -> https://opentdb.com/api_count.php?category=14
            // create backing model for this.
            // {   
            //      category_id: 14,
            //      category_question_count:
            //      {
            //          total_question_count: 170,
            //          total_easy_question_count: 69,
            //          total_medium_question_count: 72,
            //          total_hard_question_count: 29
            //      }
            // }
        }

        public async Task ImportQuestions()
        {
            string response = string.Empty;
            try
            {
                response = await client.GetStringAsync(OpenTDbAPI.Url);

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

            // Helper API Tools
            // https://opentdb.com/api_count.php?category=CATEGORY_ID_HERE

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
        //TODO: Apparently that is already solved with a session token ->
        // https://opentdb.com/api_token.php?command=request

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
