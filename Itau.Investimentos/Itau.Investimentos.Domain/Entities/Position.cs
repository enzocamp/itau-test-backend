using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    [Table("positios")]
    public class Position
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public uint Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal PnL { get; set; }
    }
}
