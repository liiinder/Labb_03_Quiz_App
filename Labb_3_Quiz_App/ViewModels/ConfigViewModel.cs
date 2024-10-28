using Labb_3_Quiz_App.Commands;
using Labb_3_Quiz_App.DataTypes;
using Labb_3_Quiz_App.Models;

namespace Labb_3_Quiz_App.ViewModels
{
    internal class ConfigViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand DeleteQuestionCommand { get; }
        //public DelegateCommand DeleteMultipleQuestionCommand { get; }
        //public DelegateCommand SelectQuestionCommand { get; }
        //TODO: fix this shit...

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

        public List<Difficulty> Difficulties { get; set; }
        public ConfigViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            DeleteQuestionCommand = new DelegateCommand(DeleteQuestion);
            //SelectQuestionCommand = new DelegateCommand(SelectQuestion);

            Difficulties = new();
            foreach (Difficulty d in Enum.GetValues(typeof(Difficulty)))
            {
                Difficulties?.Add(d);
            }

        }

        private void DeleteQuestion(object obj) => ActivePack?.Questions.Remove(SelectedQuestion);

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        private bool CanAddQuestion(object? arg) => true; // can be whatever expression and isnt needed for true...
        private void AddQuestion(object obj) => ActivePack?.Questions.Add(new Question());

    }
}
