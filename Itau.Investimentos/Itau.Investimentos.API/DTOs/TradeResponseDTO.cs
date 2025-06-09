using Itau.Investimentos.Domain.Enums;

namespace Itau.Investimentos.API.DTOs
{
    public class TradeResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public uint Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Fee { get; set; }
        public TradeType TradeType { get; set; }
        public DateTime ExecutedAt { get; set; }

        // Opcional: se quiser retornar nome do ativo ou do usuário no futuro
        public string? AssetCode { get; set; }
        public string? UserName { get; set; }
    }
}

