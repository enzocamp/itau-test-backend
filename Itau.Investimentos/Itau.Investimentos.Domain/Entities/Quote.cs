using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime QuotedAt { get; set; }
    }
}
