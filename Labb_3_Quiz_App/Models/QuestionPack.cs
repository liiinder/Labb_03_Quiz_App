using Labb_3_Quiz_App.DataTypes;
using Labb_3_Quiz_App.Models;

internal class QuestionPack
{
    public string Name { get; set; }
    public Difficulty Difficulty { get; set; }
    public int TimeLimitInSeconds { get; set; }
    public List<Question> Questions { get; set; } = new();

    public QuestionPack(string name,
                        Difficulty difficulty = Difficulty.Medium,
                        int timeLimitInSeconds = 30)
    {
        Name = name;
        Difficulty = difficulty;
        TimeLimitInSeconds = timeLimitInSeconds;
    }
}