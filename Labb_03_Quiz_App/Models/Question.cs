using System.Runtime.Serialization;

namespace Labb_03_Quiz_App.Models
{
    [DataContract]
    internal class Question
    {
        [DataMember(Name = "question")]
        public string Query { get; set; }

        [DataMember(Name = "correct_answer")]
        public string CorrectAnswer { get; set; }

        [DataMember(Name = "incorrect_answers")]
        public string[] IncorrectAnswers { get; set; }

        public Question()
        {
            Query = string.Empty;
            CorrectAnswer = string.Empty;
            IncorrectAnswers = [string.Empty, string.Empty, string.Empty];
        }

        public override string ToString() => Query;
    }
}