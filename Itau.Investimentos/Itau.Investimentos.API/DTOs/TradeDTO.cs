using Itau.Investimentos.Domain.Enums;
using System.Text.Json.Serialization;

namespace Itau.Investimentos.API.DTOs
{
    public class TradeDTO
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public int Quantity { get; set; }
        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("tradeType")]
        public TradeType TradeType { get; set; }
    }
}
