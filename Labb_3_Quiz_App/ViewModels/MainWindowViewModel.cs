using System.Collections.ObjectModel;

namespace Labb_3_Quiz_App.ViewModels
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
                RaisePropertyChanged();
                ConfigViewModel.RaisePropertyChanged("ActivePack");
            }
        }
        public ConfigViewModel ConfigViewModel { get; }
        public GameViewModel GameViewModel { get; }
        public MainWindowViewModel()
        {
            ConfigViewModel = new ConfigViewModel(this);
            GameViewModel = new GameViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));
            Packs = new ObservableCollection<QuestionPackViewModel>();
        }

    }
}
