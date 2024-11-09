using Labb_03_Quiz_App.Commands;
using Labb_03_Quiz_App.Models;
using System.Diagnostics;
using System.Windows.Threading;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private int _amountOfQuestions;
        private int _questionIndex;
        private int _correctGuesses;
        private bool _hasGuessed;
        private int? _timeLeft;
        private bool? _resultScreen;
        private int TimeBetweenQuestions = 2;

        public bool IsActive { get => (mainWindowViewModel is null ? false : mainWindowViewModel.InGameMode); }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        public Stack<Question>? UnansweredQuestions { get; set; }
        public Question ActiveQuestion { get; set; }
        public List<Answer> ActiveAnswers { get; set; }

        public int AmountOfQuestions
        {
            get => _amountOfQuestions;
            set
            {
                _amountOfQuestions = value;
                RaisePropertyChanged();
            }
        }
        public int QuestionIndex
        {
            get => _questionIndex;
            set
            {
                _questionIndex = value;
                RaisePropertyChanged();
            }
        }
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
        public int? TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
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

        public DelegateCommand GuessAnswerCommand { get; }
        public DelegateCommand StartQuizCommand { get; }

        public GameViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            GuessAnswerCommand = new DelegateCommand(GuessAnswer);
            StartQuizCommand = new DelegateCommand(StartQuiz);

            this.mainWindowViewModel = mainWindowViewModel;

            ActiveQuestion = new Question();
            ActiveAnswers = new List<Answer>(4);
            for (int i = 0; i < 4; i++) ActiveAnswers.Add(new Answer(""));

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void GuessAnswer(object guess)
        {
            if (!HasGuessed)
            {
                ActiveAnswers.First(a => a.Text == ActiveQuestion.CorrectAnswer).Color = "Green";

                if (guess is Answer a)
                {
                    if (a.Color == "Green") CorrectGuesses++;
                    else a.Color = "Red";
                }

                RaisePropertyChanged("ActiveAnswers");

                TimeLeft = TimeBetweenQuestions;
                timer.Start();
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
            QuestionIndex = ActivePack.Questions.Count() - UnansweredQuestions.Count() + 1;
            if (UnansweredQuestions?.Count > 0)
            {
                HasGuessed = false;

                ActiveQuestion = UnansweredQuestions.Pop();
                RaisePropertyChanged("ActiveQuestion");

                ActiveAnswers = ShuffledAnswers(ActiveQuestion);
                RaisePropertyChanged("ActiveAnswers");

                TimeLeft = ActivePack?.TimeLimitInSeconds;
            }
            else
            {
                Debug.WriteLine(ActivePack);
                Debug.WriteLine(mainWindowViewModel.Packs.Count);
                UnansweredQuestions = null;
                ResultScreen = true;
                timer.Stop();
            }
            RaisePropertyChanged("UnansweredQuestions");
        }
        private void StartQuiz(object obj)
        {
            ResultScreen = null;
            AmountOfQuestions = ActivePack.Questions.Count();
            CorrectGuesses = 0;

            var tempListOfQuestions = new List<Question>();
            foreach (Question q in ActivePack.Questions) tempListOfQuestions.Add(q);

            UnansweredQuestions = new Stack<Question>(ShuffleList(tempListOfQuestions));
            RaisePropertyChanged("UnansweredQuestions");

            NextQuestion();
        }
        private List<T> ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                int random = new Random().Next(i, list.Count());
                var temp = list[i];
                list[i] = list[random];
                list[random] = temp;
            }
            return list;
        }
        private List<Answer> ShuffledAnswers(Question q)
        {
            var answers = new List<Answer>();
            answers.Add(new Answer(q.CorrectAnswer));
            foreach (string text in q.IncorrectAnswers) answers.Add(new Answer(text));

            return ShuffleList(answers);
        }
    }
}
