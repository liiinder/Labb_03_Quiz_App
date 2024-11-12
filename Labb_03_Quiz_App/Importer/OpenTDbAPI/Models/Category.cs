using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI.Models
{
    internal class Category
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Category()
        {
            Id = 0;
            Name = string.Empty;
        }
    }
}