namespace Itau.Investimentos.API.DTOs
{
    public class QuoteResponseDTO
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime QuotedAt { get; set; }

        // Opcional: incluir código do ativo, se necessário
        public string? AssetCode { get; set; }
    }
}
