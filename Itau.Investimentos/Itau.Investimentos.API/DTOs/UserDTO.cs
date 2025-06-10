using System.Text.Json.Serialization;

namespace Itau.Investimentos.API.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("fee_percentage")]
        public decimal FeePercentage { get; set; }
    }
}
