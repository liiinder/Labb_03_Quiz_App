namespace Labb_03_Quiz_App.Models
{
    internal class OpenTDb
    {
        public int response_code;

        public List<Question> results;

        public OpenTDb(int response_code, List<Question> results)
        {
            this.response_code = response_code;
            this.results = results;
        }
    }
}

//TODO: Currently not in use... Will be used with ImportQuestions