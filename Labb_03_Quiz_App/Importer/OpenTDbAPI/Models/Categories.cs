using Labb_03_Quiz_App.Models;
using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI.Models
{
    class OpenTDbCategories
    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> ListOfCategories { get; set; }

        public OpenTDbCategories() { ListOfCategories = new List<Category>(); }
    }
}
