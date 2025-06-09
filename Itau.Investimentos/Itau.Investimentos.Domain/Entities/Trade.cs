using Itau.Investimentos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    [Table("trades")]
    public class Trade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("asset_id")]
        public int AssetId { get; set; }

        [Column("quantity")]
        public uint Quantity { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("trade_type")]
        public TradeType TradeType { get; set; }

        [Column("fee")]
        public decimal Fee { get; set; }

        [Column("executed_at")]
        public DateTime ExecutedAt { get; set; }
        public Asset? Asset { get; set; }
        public User? User { get; set; }
    }
}
