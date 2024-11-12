using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Importer.OpenTDbAPI.Models;
using Labb_03_Quiz_App.Models;
using Labb_03_Quiz_App.ViewModels;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI
{
    internal class OpenTDbAPI
    {
        private static readonly HttpClient client = new HttpClient() { BaseAddress = new Uri("https://opentdb.com/") };

        public Category ChoosenCategory { get; set; }
        public Difficulty Difficulty { get; set; }
        public int AmountOfQuestions { get; set; }
        public Dictionary<string, QuestionPackViewModel> TokenDict { get; set; }

        public DateTime LatestImport { get; set; }
        public Dictionary<int, string> ResponseMessages { get; set; }
        public List<Category> Categories { get; set; }

        public OpenTDbAPI()
        {
            TokenDict = new Dictionary<string, QuestionPackViewModel>();

            LatestImport = DateTime.Now.Subtract(TimeSpan.FromSeconds(10));

            Categories = new List<Category>();
            Categories.Add(new Category(0, "Any"));

            ChoosenCategory = Categories[0];
            Difficulty = Difficulty.Any;
            AmountOfQuestions = 5;

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

        public string GetUrl(string? token)
        {
            token = (token is null) ? "" : $"&token={token}";
            string difficulty = (Difficulty == Difficulty.Any) ? "" : $"&difficulty={Difficulty.ToString().ToLower()}";
            string category = (ChoosenCategory.Id == 0) ? "" : $"&category={ChoosenCategory.Id}";

            return $"api.php?amount={AmountOfQuestions}{category}{difficulty}&type=multiple{token}";
        }

        public async Task<bool> ImportCategories()
        {
            try
            {
                var response = await client.GetStringAsync("api_category.php");

                var deserialized = JsonSerializer.Deserialize<Categories>(response);

                if (deserialized is not null)
                {
                    foreach (Category cat in deserialized.ListOfCategories) Categories.Add(cat);
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Failed connection to OpenTDb, check your internet connection.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public async Task<string?> GetToken(QuestionPackViewModel pack)
        {
            string? token = TokenDict.FirstOrDefault(x => x.Value == pack).Key;

            if (token is null)
            {
                try
                {
                    var response = await client.GetStringAsync("api_token.php?command=request");
                    var deserialized = JsonSerializer.Deserialize<Token>(response);

                    if (deserialized is not null) TokenDict[deserialized.token] = pack;
                    return deserialized?.token;
                }
                catch
                {
                    MessageBox.Show("Failed Token request, check your internet connection.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else return token;

            return null;
        }

        public async Task<bool> ImportQuestions(QuestionPackViewModel pack)
        {
            string? token = await GetToken(pack);

            if (token is not null)
            {
                string response = string.Empty;
                Questions? deserialized = new();
                string url = GetUrl(token);

                // Seems like OpenTDb autoreject instead of sending response_code: 5 when you use HttpClient (works in chrome)
                if (DateTime.Now.Subtract(LatestImport) > TimeSpan.FromMilliseconds(5000))
                {
                    try
                    {
                        LatestImport = DateTime.Now;
                        response = await client.GetStringAsync(url);
                        deserialized = JsonSerializer.Deserialize<Questions>(response);

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
                        if (deserialized?.Response is int responseId)
                        {
                            if (responseId == 0) return true;
                            MessageBox.Show(ResponseMessages[responseId], "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch { MessageBox.Show("Failed connection to OpenTDb, check your internet connection.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
                else MessageBox.Show(ResponseMessages[5], "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else MessageBox.Show("No active token.", "OpenTDb", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
}