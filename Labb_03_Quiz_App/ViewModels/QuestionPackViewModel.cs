﻿using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Models;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.ViewModels
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

        [JsonConstructorAttribute]
        public QuestionPackViewModel(string name, Difficulty difficulty, int timeLimitInSeconds, ObservableCollection<Question> questions)
        {
            model = new QuestionPack();
            Questions = questions;
            model.Name = name;
            model.Difficulty = difficulty;
            model.TimeLimitInSeconds = timeLimitInSeconds;
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