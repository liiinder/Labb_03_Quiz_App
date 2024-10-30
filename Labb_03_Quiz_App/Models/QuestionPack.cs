using Labb_03_Quiz_App.DataTypes;
using Labb_03_Quiz_App.Models;

internal class QuestionPack
{
    public string Name { get; set; }
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }
    public List<Question> Questions { get; set; }

    public QuestionPack()
    {
        Questions = new List<Question>();
        Name = string.Empty;
        Difficulty = Difficulty.Medium;
        TimeLimitInSeconds = 30;
    }
}