using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Models
{
    class Categories
    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> ListOfCategories { get; set; }

        public Categories() { ListOfCategories = new List<Category>(); }
    }
}
