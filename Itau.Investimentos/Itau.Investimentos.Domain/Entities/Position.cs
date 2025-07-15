using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    [Table("positions")]
    public class Position
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("asset_id")]
        public int AssetId { get; set; }
        [Required]
        [Column("quantity")]
        public uint Quantity { get; set; }
        [Required]
        [Column("average_price")]
        public decimal AveragePrice { get; set; }
        [Required]
        [Column("pnl")]
        public decimal PnL { get; set; }

        public User? User { get; set; }
        public Asset? Asset { get; set; }
    }
}
