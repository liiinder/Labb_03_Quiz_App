﻿using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Data;
using Labb_03_Quiz_App.Dialogs;
using Labb_03_Quiz_App.Importer.OpenTDbAPI;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
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
                RaisePropertyChanged("NoPacks");
            }
        }
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigViewModel ConfigViewModel { get; }
        public GameViewModel GameViewModel { get; }
        public OpenTDbAPI OpenTDbAPI { get; set; }
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
                RaisePropertyChanged("CanImportOrPlay");
                GameViewModel.RaisePropertyChanged("IsActive");
                if (ActivePack is not null) GameViewModel.StartQuizCommand.Execute(null);
            }
        }

        //TODO: Clean up props/fields
        // like the "NoPacks/LoadedPacks/HasImportedCategories/CanImportOrPlay...

        // and add options to disable menu options if conditions is not met.
        // delete question menu option if any is selected
        // Give the "CanImportOrPlay" a better name as it apparently is used for almost all config things.
        // Or look into other ways to do theses bindings without special props.

        //TODO: Test out more with the command methods CanExecute() / RaiseCanExecuteChanged()
        // Would help with the above TODO.

        public bool CanImportOrPlay { get => (InConfigMode && ActivePack is not null); }
        public bool NoPacks { get => (ActivePack is null && LoadedPacks == true); }
        public bool LoadedPacks { get; set; }
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

            OpenTDbAPI = new OpenTDbAPI();

            AddNewPackCommand = new DelegateCommand(AddNewPack);
            SelectPackCommand = new DelegateCommand(SelectPack);
            DeletePackCommand = new DelegateCommand(DeletePack);
            OpenImportQuestionsDialogCommand = new DelegateCommand(OpenImportQuestionDialog);
            ExitWindowCommand = new DelegateCommand(ExitWindow);
            SwitchModeCommand = new DelegateCommand(SwitchMode);
            FullScreenCommand = new DelegateCommand(FullScreen);
        }

        private void ExitWindow(object obj)
        {
            JsonHandler json = new();
            json.SaveToFile(Packs);
            Environment.Exit(0);
        }

        private void AddNewPack(object obj)
        {
            ActivePack = new QuestionPackViewModel(new QuestionPack());
            Packs.Add(ActivePack);
            new PackOptionsDialog().ShowDialog();
        }
        private void SelectPack(object obj)
        {
            if (obj is QuestionPackViewModel q && ActivePack != q) ActivePack = q;
        }
        private void DeletePack(object obj)
        {
            Packs.Remove(ActivePack);
            ActivePack = (Packs.Count > 0) ? Packs[^1] : null;
        }

        private void OpenImportQuestionDialog(object obj)
        {
            new FetchQuestionsDialog().ShowDialog();
            //TODO: Look into options of refactor dialog from ViewModel.
            // Try out this solution? https://www.youtube.com/watch?app=desktop&v=S8hEjLahNtU
            // Would also help with the dialog in "AddNewPack"
            // Might also work to clean up the SwitchMode method below.
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
