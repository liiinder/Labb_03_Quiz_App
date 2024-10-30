using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.View.Windows;
using System.Collections.ObjectModel;
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
                ConfigViewModel.RaisePropertyChanged("ActivePack");
            }
        }
        public ConfigViewModel ConfigViewModel { get; }
        public GameViewModel GameViewModel { get; }

        public DelegateCommand AddNewPackCommand { get; }
        public DelegateCommand SelectPackCommand { get; }
        public DelegateCommand DeletePackCommand { get; }
        public DelegateCommand SwitchModeCommand { get; }
        public DelegateCommand ExitWindowCommand { get; }

        public MainWindowViewModel()
        {
            ConfigViewModel = new ConfigViewModel(this);
            GameViewModel = new GameViewModel(this);

            ActivePack = null;
            Packs = new ObservableCollection<QuestionPackViewModel>();
            LoadPacks();

            AddNewPackCommand = new DelegateCommand(AddNewPack);
            SelectPackCommand = new DelegateCommand(SelectPack);
            DeletePackCommand = new DelegateCommand(DeletePack);
            SwitchModeCommand = new DelegateCommand(SwitchMode);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
        }

        private void SwitchMode(object obj)
        {
            if (obj is string button)
            {
                if (button == "ConfigMode")
                {
                    ConfigViewModel.IsVisible = Visibility.Visible;
                    GameViewModel.IsVisible = Visibility.Hidden;
                }
                else if (button == "GameMode")
                {
                    ConfigViewModel.IsVisible = Visibility.Hidden;
                    GameViewModel.IsVisible = Visibility.Visible;
                }
            }
            //TODO: Kolla om man kan fixa detta så man istället blir en aktiv viewmodel istället och ändra dom andra till null
            // Sen göra visibility kollen mot null istället med samma konverter som används till annat.
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

        //
        // Save and Load to json...
        //

        private static string pathToFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Labb_03_Quiz_App";
        private static string pathToFile = pathToFolder + "\\QuestionPacks.json";

        private void ExitWindow(object obj)
        {
            string jsonString = JsonSerializer.Serialize(Packs);
            File.WriteAllText(pathToFile, jsonString);
            Environment.Exit(0);
        }

        public void LoadPacks()
        {
            Directory.CreateDirectory(pathToFolder);

            if (!File.Exists(pathToFile))
            {
                var emptyPacks = new ObservableCollection<QuestionPackViewModel>();
                string jsonString = JsonSerializer.Serialize(emptyPacks);
                File.WriteAllText(pathToFile, jsonString);
            }

            string json = File.ReadAllText(pathToFile);
            var loadedPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(json);

            foreach (QuestionPackViewModel pack in loadedPacks) Packs.Add(pack);

            if (ActivePack is null && Packs.Count > 0) ActivePack = Packs[^1];
        }
    }
}
