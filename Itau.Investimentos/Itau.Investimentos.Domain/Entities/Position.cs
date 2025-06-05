using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
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
