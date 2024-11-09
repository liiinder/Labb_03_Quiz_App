using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Models
{
    internal class Question
    {
        [JsonPropertyName("question")]
        public string Query { get; set; }
        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }
        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; set; }

        public Question()
        {
            Query = string.Empty;
            CorrectAnswer = string.Empty;
            IncorrectAnswers = [string.Empty, string.Empty, string.Empty];
        }

        [JsonConstructor]
        public Question(string query, string correctAnswer, string[] incorrectAnswers)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }

        public override string ToString() => Query;
    }
}