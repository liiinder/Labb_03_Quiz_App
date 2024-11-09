using Labb_03_Quiz_App.DataTypes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Models
{
    internal class OpenTDb : INotifyPropertyChanged
    {
        private Category _cat;
        private Difficulty _difficulty;
        private int _amountOfQuestions;

        [JsonPropertyName("response_code")]
        public int Response { get; set; }
        [JsonPropertyName("results")]
        public List<Question> ListOfQuestions { get; set; }

        // Testing ...

        [JsonIgnore]
        public Category Cat
        {
            get => _cat;
            set
            {
                _cat = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Url");
            }
        }
        [JsonIgnore]
        public Difficulty Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Url");
            }
        }
        [JsonIgnore]
        public int AmountOfQuestions
        {
            get => _amountOfQuestions;
            set
            {
                _amountOfQuestions = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Url");
            }
        }

        [JsonIgnore]
        public string Url
        {
            get
            {
                string difficulty = (Difficulty == Difficulty.Any) ? "" : $"&difficulty={Difficulty.ToString().ToLower()}";
                string category = (Cat.Id == 0) ? "" : $"&category={Cat.Id}";

                return $"api.php?amount={AmountOfQuestions}{category}{difficulty}&type=multiple";
            }
        }

        public OpenTDb()
        {
            Response = 0;
            ListOfQuestions = new List<Question>();
            _difficulty = Difficulty.Any;
            _cat = new Category();
            _amountOfQuestions = 5;
        }

        public OpenTDb(Category cat) : this()
        {
            _cat = cat;
        }

        //TODO: Bryta upp denna till Model/ViewModel, men ligger så här nu för att få det att få helheten att fungera först.

        public event PropertyChangedEventHandler? PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

//TODO: Currently not in use... Will be used with ImportQuestions