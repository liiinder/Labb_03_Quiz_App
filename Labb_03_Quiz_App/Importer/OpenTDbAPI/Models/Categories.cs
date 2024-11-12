using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI.Models
{
    class Categories
    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> ListOfCategories { get; set; }

        public Categories() { ListOfCategories = new List<Category>(); }
    }
}
