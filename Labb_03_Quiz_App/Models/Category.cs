using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Models
{
    internal class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public string Url { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
            Url = (id > 0) ? $"&category={id}" : "";
        }
    }
}