using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Models;
using Labb_03_Quiz_App.View.Windows;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class ConfigViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        public bool? IsActive { get => mainWindowViewModel?.InConfigMode; }
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

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand DeleteQuestionCommand { get; }
        public DelegateCommand PackOptionsCommand { get; }
        public DelegateCommand SwapQuestionCommand { get; }

        public ConfigViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(AddQuestion);
            DeleteQuestionCommand = new DelegateCommand(DeleteQuestion);
            PackOptionsCommand = new DelegateCommand(PackOptions);
            SwapQuestionCommand = new DelegateCommand(SwapQuestion);
            Difficulties = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();
        }

        private void AddQuestion(object obj)
        {
            ActivePack?.Questions.Add(new Question());
            SelectedQuestion = ActivePack?.Questions[^1];
        }

        private void DeleteQuestion(object obj)
        {
            int SelectedIndex = ActivePack.Questions.IndexOf(SelectedQuestion);
            ActivePack?.Questions.Remove(SelectedQuestion);

            if (ActivePack?.Questions.Count > 0)
            {
                if (SelectedIndex == ActivePack?.Questions.Count || SelectedIndex == -1)
                {
                    SelectedQuestion = ActivePack?.Questions[^1];
                }
                else
                {
                    SelectedQuestion = ActivePack?.Questions[SelectedIndex];
                }
            }
        }

        private void SwapQuestion(object obj)
        {
            int SelectedIndex = ActivePack.Questions.IndexOf(SelectedQuestion);

            if (SelectedQuestion == ActivePack?.Questions[^1])
            {
                AddQuestion(null);
            }
            else SelectedQuestion = ActivePack?.Questions[SelectedIndex + 1];
        }

        private void PackOptions(object obj) => new PackOptions().ShowDialog();
    }
}
