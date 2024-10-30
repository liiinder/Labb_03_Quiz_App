using Labb_03_Quiz_App.Commands;
using System.Windows;
using System.Windows.Threading;

namespace Labb_03_Quiz_App.ViewModels
{
    internal class GameViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;

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

        // Sen blanda ordningen på frågorna... Och innan man skriver ut svarsalternativen blanda dom.

        public QuestionPackViewModel? ActivePack { get; set; }

        private string _testData;
        public string TestData
        {
            get => _testData;
            private set
            {
                _testData = value;
                RaisePropertyChanged();
            }
        }

        private int _timeLeft;
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                _timeLeft = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateButtonCommand { get; }

        public GameViewModel(MainWindowViewModel? mainWindowViewModel)
        {

            ActivePack = mainWindowViewModel?.ActivePack;
            this.mainWindowViewModel = mainWindowViewModel;

            _testData = "Start Value";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();
            IsVisible = Visibility.Hidden;
            UpdateButtonCommand = new DelegateCommand(UpdateButton, CanUpdateButton);
        }

        private bool CanUpdateButton(object? arg) => TestData.Length < 20; // if you can update button

        private void UpdateButton(object obj)
        {
            TestData += "hej";
            UpdateButtonCommand.RaiseCanExecuteChanged();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestData += "x";
            // TimeLeft > 0 ...
            // if TimeLeft == 0 , Answers().
            // then NextQuestion(). (reset TimeLeft)
        }
    }
}
