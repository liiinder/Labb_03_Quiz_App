using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Models;
using Labb_03_Quiz_App.ViewModels;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI
{
    internal class OpenTDbAPI : INotifyPropertyChanged
    {
        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://opentdb.com/") };

        private Category _choosenCategory;
        private Difficulty _difficulty;
        private int _amountOfQuestions;
        private string _token;

        [JsonPropertyName("response_code")]
        public int? Response { get; set; }

        [JsonPropertyName("response_message")]
        public string ResponseMessage { get; set; }

        [JsonIgnore]
        public Dictionary<int, string> ResponseMessages { get; set; }

        [JsonPropertyName("results")]
        public List<Question> ListOfQuestions { get; set; }

        [JsonPropertyName("token")]
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Url");
            }
        }

        [JsonIgnore]
        public Category ChoosenCategory
        {
            get => _choosenCategory;
            set
            {
                _choosenCategory = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Url");
            }
        }

        [JsonIgnore]
        public List<Category> Categories { get; set; }

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
                string category = (ChoosenCategory.Id == 0) ? "" : $"&category={ChoosenCategory.Id}";
                string token = (Token == string.Empty) ? "" : $"&token={Token}";

                return $"api.php?amount={AmountOfQuestions}{category}{difficulty}{token}&type=multiple";
            }
        }

        public OpenTDbAPI()
        {
            Response = null;
            ResponseMessage = string.Empty;
            _token = string.Empty;
            ListOfQuestions = new List<Question>();
            _difficulty = Difficulty.Any;
            _choosenCategory = new Category();
            Categories = new List<Category>();
            _amountOfQuestions = 5;
            ResponseMessages = new Dictionary<int, string>()
            {
                { 0, "Success Returned results successfully." },
                { 1, "No Results Could not return results.The API doesn't have enough questions for your query. (Ex. Asking for 50 Questions in a Category that only has 20." },
                { 2, "Invalid Parameter Contains an invalid parameter. Arguements passed in aren't valid. (Ex. Amount = Five)" },
                { 3, "Token Not Found Session Token does not exist." },
                { 4, "Token Empty Session Token has returned all possible questions for the specified query.Resetting the Token is necessary." },
                { 5, "Rate Limit Too many requests have occurred.Each IP can only access the API once every 5 seconds." }
            };
        }

        public OpenTDbAPI(Category cat) : this()
        {
            _choosenCategory = cat;
        }

        // Move list of categories here.

        //TODO: Bryta upp denna till Model/ViewModel, men ligger så här... nu för att få det att få helheten att fungera först.
        // Eller lägga som en separat grej i någon /Services mapp och ha interface samt returer.

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        public async Task<bool> ImportCategories()
        {
            try
            {
                var response = await client.GetStringAsync("api_category.php");

                var jsonDeserialized = JsonSerializer.Deserialize<Categories>(response);

                // temporary test... 
                List<Category> Categories = new();
                if (jsonDeserialized is not null)
                {
                    foreach (Category cat in jsonDeserialized.ListOfCategories) Categories.Add(cat);
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Failed connection to OpenTDb, check your internet connection.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public async Task<QuestionPackViewModel> ImportQuestions(QuestionPackViewModel pack)
        {
            string response = string.Empty;
            OpenTDbAPI deserialized = new();

            //TODO: Add token check.
            try
            {
                response = await client.GetStringAsync(Url);
                deserialized = JsonSerializer.Deserialize<OpenTDbAPI>(response);

                if (deserialized is not null && deserialized.Response == 0)
                {
                    foreach (Question q in deserialized.ListOfQuestions)
                    {
                        Question temp = new();
                        temp.Query = WebUtility.HtmlDecode(q.Query);
                        temp.CorrectAnswer = WebUtility.HtmlDecode(q.CorrectAnswer);
                        temp.IncorrectAnswers = q.IncorrectAnswers.Select(item => WebUtility.HtmlDecode(item)).ToArray();
                        pack.Questions.Add(temp);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed connection to OpenTDb, check your internet connection.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (deserialized?.Response is int responseId)
            {
                if (responseId == 0)
                {
                    MessageBox.Show(deserialized?.ResponseMessages[responseId], "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else MessageBox.Show(deserialized?.ResponseMessages[responseId], "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return pack;
        }

    }
}