﻿using Labb_3_Quiz_App.DataTypes;
using Labb_3_Quiz_App.Models;
using System.Collections.ObjectModel;

namespace Labb_3_Quiz_App.ViewModels
{
    internal class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;
        public ObservableCollection<Question> Questions { get; }

        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            Questions = new ObservableCollection<Question>(model.Questions);
        }

        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
    }
}