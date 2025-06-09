namespace Itau.Investimentos.API.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal FeePercentage { get; set; }
    }
}
