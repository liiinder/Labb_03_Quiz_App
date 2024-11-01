﻿using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Models;
using Labb_03_Quiz_App.View.Windows;
using System.Windows;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class ConfigViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        public List<Difficulty> Difficulties { get; set; }

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _isVisible;
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand DeleteQuestionCommand { get; }
        public DelegateCommand PackOptionsCommand { get; }

        public ConfigViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(AddQuestion);
            DeleteQuestionCommand = new DelegateCommand(DeleteQuestion);
            PackOptionsCommand = new DelegateCommand(PackOptions);
            Difficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();
            IsVisible = Visibility.Visible;
        }

        private void AddQuestion(object obj) => ActivePack?.Questions.Add(new Question());
        private void DeleteQuestion(object obj) => ActivePack?.Questions.Remove(SelectedQuestion);
        private void PackOptions(object obj) => new PackOptions().ShowDialog();
    }
}
