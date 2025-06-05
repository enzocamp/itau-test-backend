using Itau.Investimentos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    public class Trade
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public uint Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public TradeType TradeType { get; set; }
        public decimal Fee { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}
