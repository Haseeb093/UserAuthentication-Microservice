using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Domain.Models
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }

        [StringLength(2)]
        [Column(TypeName = "char")]
        public string IsoCode { get; set; }

        [StringLength(3)]
        [Column(TypeName = "char")]
        public string Code { get; set; }

        [StringLength(80)]
        public string Name { get; set; }
        public int? NumCode { get; set; }
        public int? PhoneCode { get; set; }
    }
}
