using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Models;
using System.Windows.Threading;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private bool _hasGuessed;
        private int? _timeLeft;
        private int _whichQuestion;
        private int _amountOfQuestions;
        private int _correctGuesses;
        private bool? _resultScreen;
        private int TimeBetweenQuestions = 2;

        public bool? IsActive { get => mainWindowViewModel?.InGameMode; }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        public Stack<Question>? ShuffledUnansweredQuestions { get; set; }
        public Question ActiveQuestion { get; set; }
        public List<Answer> ActiveAnswers { get; set; }

        public int CorrectGuesses
        {
            get => _correctGuesses;
            set
            {
                _correctGuesses = value;
                RaisePropertyChanged();
            }
        }
        public bool HasGuessed
        {
            get => _hasGuessed;
            set
            {
                _hasGuessed = value;
                RaisePropertyChanged();
            }
        }
        public bool? ResultScreen
        {
            get => _resultScreen;
            set
            {
                _resultScreen = value;
                RaisePropertyChanged();
            }
        }
        public int? TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                RaisePropertyChanged();
            }
        }
        public int WhichQuestion
        {
            get => _whichQuestion;
            set
            {
                _whichQuestion = value;
                RaisePropertyChanged();
            }
        }
        public int AmountOfQuestions
        {
            get => _amountOfQuestions;
            set
            {
                _amountOfQuestions = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand GuessAnswerCommand { get; }
        public DelegateCommand RepeatQuizCommand { get; }

        public GameViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            GuessAnswerCommand = new DelegateCommand(GuessAnswer);
            RepeatQuizCommand = new DelegateCommand(RepeatQuiz);

            this.mainWindowViewModel = mainWindowViewModel;

            ActiveAnswers = new List<Answer> { new Answer("Answer A"),
                                               new Answer("Answer B"),
                                               new Answer("Answer C"),
                                               new Answer("Answer D")};
            ActiveQuestion = new Question();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void RepeatQuiz(object obj)
        {
            StartQuiz();
        }

        private void GuessAnswer(object obj)
        {
            if (!HasGuessed)
            {
                foreach (Answer ans in ActiveAnswers)
                {
                    if (ans.Text == ActiveQuestion.CorrectAnswer) ans.Color = "Green";
                }

                if (obj is Answer a)
                {
                    if (a.Text == ActiveQuestion.CorrectAnswer) CorrectGuesses++;
                    else a.Color = "Red";
                }
                TimeLeft = TimeBetweenQuestions; // The time the right answer is displayed
                timer.Start();
                RaisePropertyChanged("ActiveAnswers");
            }
            HasGuessed = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
            if (TimeLeft == 0 && !HasGuessed) GuessAnswer(e);
            else if (TimeLeft == 0 && HasGuessed) NextQuestion();
        }

        private void NextQuestion()
        {
            WhichQuestion++;
            if (ShuffledUnansweredQuestions?.Count > 0)
            {
                HasGuessed = false;

                ActiveQuestion = ShuffledUnansweredQuestions.Pop();
                RaisePropertyChanged("ActiveQuestion");

                ActiveAnswers = ShuffledAnswers(ActiveQuestion);
                RaisePropertyChanged("ActiveAnswers");

                TimeLeft = ActivePack?.TimeLimitInSeconds;
            }
            else
            {
                ShuffledUnansweredQuestions = null;
                ResultScreen = true;
            }
            RaisePropertyChanged("ShuffledUnansweredQuestions");
        }

        public void StartQuiz()
        {
            ResultScreen = null;
            AmountOfQuestions = ActivePack.Questions.Count();
            CorrectGuesses = 0;
            WhichQuestion = 0;

            var tempListOfQuestions = new List<Question>();
            foreach (Question q in ActivePack.Questions)
            {
                tempListOfQuestions.Add(q);
            }
            ShuffleList(tempListOfQuestions);
            ShuffledUnansweredQuestions = new Stack<Question>(tempListOfQuestions);
            RaisePropertyChanged("ShuffledQuestions");

            NextQuestion();
        }

        private void ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                int random = new Random().Next(i, list.Count());
                var temp = list[i];
                list[i] = list[random];
                list[random] = temp;
            }
        }
        private List<Answer> ShuffledAnswers(Question q)
        {
            var answers = new List<Answer>();
            answers.Add(new Answer(q.CorrectAnswer));
            foreach (string s in q.IncorrectAnswers) answers.Add(new Answer(s));
            ShuffleList(answers);
            return answers;
        }
    }
}
