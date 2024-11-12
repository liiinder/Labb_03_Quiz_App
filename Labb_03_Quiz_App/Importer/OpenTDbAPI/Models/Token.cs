using System.Text.Json.Serialization;

namespace Labb_03_Quiz_App.Importer.OpenTDbAPI.Models
{
    internal class Token
    {
        [JsonPropertyName("response_code")]
        public int response_code { get; set; }
        [JsonPropertyName("response_message")]
        public string response_message { get; set; }
        [JsonPropertyName("token")]
        public string token { get; set; }

        [JsonConstructor]
        public Token(int response_code, string response_message, string token)
        {
            this.response_code = response_code;
            this.response_message = response_message;
            this.token = token;
        }
    }
}
