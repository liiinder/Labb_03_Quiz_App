using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.View.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

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

        public MainWindowViewModel()
        {
            ConfigViewModel = new ConfigViewModel(this);
            GameViewModel = new GameViewModel(this);

            InConfigMode = true;
            CurrentMode = "ConfigMode";

            ActivePack = null;
            Packs = new ObservableCollection<QuestionPackViewModel>();
            LoadPacks();

            AddNewPackCommand = new DelegateCommand(AddNewPack);
            SelectPackCommand = new DelegateCommand(SelectPack);
            DeletePackCommand = new DelegateCommand(DeletePack);
            SwitchModeCommand = new DelegateCommand(SwitchMode);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
            ImportQuestionsCommand = new DelegateCommand(ImportQuestions);
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

            //TODO: Remove the new PackOptions().ShowDialog(); from the ViewModel...
            // But where to place it?
            // https://stackoverflow.com/questions/18435173/open-close-view-from-viewmodel

            // Hampus Eiderström Swahn:
            // Har suttit hela dagen med detta, Fredrik hintade om events i något meddelande i vår teams.
            // Så nu skickar mina commands ut ett event som view väljer att prenumerera på för att de ska
            // veta när det är läge att öppna dialoger.
            // Funkar bra, känns mvvm och möjliggör för keybindings och hela den fadderullan

            // Eller kolla på RelayCommand som Jens använder
            // https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/relaycommand

            // Jens Eresund: och sen för att binda det till knappen medans man fortfarande har viewmodel
            // som datacontext så behöver man göra så här:
            // Command = "{Binding Path=OpenQuizCommand, RelativeSource={RelativeSource Mode=FindAncestor,
            //                  AncestorLevel=1, AncestorType=local:ConfigurationView} }"
        }

        //
        // Save and Load to json...
        //

        private static string pathToFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb_03_Quiz_App";
        private static string pathToFile = pathToFolder + "\\QuestionPacks.json";

        public void ExitWindow(object obj)
        {
            string jsonString = JsonSerializer.Serialize(Packs);
            File.WriteAllText(pathToFile, jsonString);
            Environment.Exit(0);

            //TODO: change this to SavePacks() and move the Exit functionality out of the VM to the view
        }

        public void LoadPacks()
        {
            Directory.CreateDirectory(pathToFolder); // Creates the directory if it doesnt exist otherwise nothing

            if (!File.Exists(pathToFile)) // Creates an file with an empty object if it doesnt exist.
            {
                //var emptyPacks = new ObservableCollection<QuestionPackViewModel>();
                //string jsonString = JsonSerializer.Serialize("[]");
                File.WriteAllText(pathToFile, "[]");
            }

            string json = File.ReadAllText(pathToFile);
            var loadedPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);

            foreach (QuestionPackViewModel pack in loadedPacks) Packs.Add(pack);

            if (ActivePack is null && Packs.Count > 0) ActivePack = Packs[^1];
        }

        public void ImportQuestions(object obj)
        {
            string json = File.ReadAllText(pathToFolder + "\\Hardcoded_API_Response.json");
            var importedQuestions = JsonSerializer.Deserialize<List<T>>(json);
            Debug.WriteLine(json[0]);
            Debug.WriteLine(json[0]);
        }
    }
}
