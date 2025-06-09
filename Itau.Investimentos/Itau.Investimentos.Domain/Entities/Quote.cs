using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    [Table("quotes")]
    public class Quote
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("asset_id")]
        public int AssetId { get; set; }
        [Required]
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Column("quoted_at")]
        public DateTime QuotedAt { get; set; }

        // Relacionamento
        public Asset? Asset { get; set; }
    }
}
