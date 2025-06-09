using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Investimentos.Domain.Entities
{
    [Table("assets")]
    public class Asset
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        public string Code { get; set; } = string.Empty;
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}
