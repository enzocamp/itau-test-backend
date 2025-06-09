namespace Itau.Investimentos.API.DTOs
{
    public class UserResponseDTO
    {     
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal FeePercentage { get; set; }
    }
}

