using Labb_03_Quiz_App.Models;
using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI.Models
{
    internal class Questions
    {
        [JsonPropertyName("results")]
        public List<Question> ListOfQuestions { get; set; }

        [JsonPropertyName("response_code")]
        public int? Response { get; set; }
    }
}
