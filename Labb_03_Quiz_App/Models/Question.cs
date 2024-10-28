namespace Labb_03_Quiz_App.Models
{
    internal class Question
    {
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }

        public Question()
        {
            Query = string.Empty;
            CorrectAnswer = string.Empty;
            IncorrectAnswers = [string.Empty, string.Empty, string.Empty];
        }
    }
}