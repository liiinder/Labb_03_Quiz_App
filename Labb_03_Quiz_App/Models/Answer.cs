namespace Labb_03_Quiz_App.Models
{
    internal class Answer
    {
        public string Text { get; set; }
        public string Color { get; set; }

        public Answer(string text, string color = "Grey")
        {
            Text = text;
            Color = color;
        }

        public override string ToString() => Text;
    }
}