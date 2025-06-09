namespace Itau.Investimentos.API.DTOs
{
    public class QuoteDTO
    {
        public int AssetId { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime QuotedAt { get; set; }
    }
}
