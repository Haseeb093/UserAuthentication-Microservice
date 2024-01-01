using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Domain.Models
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        [Column(TypeName = "char")]
        [StringLength(2)]
        public string? IsoCode { get; set; }
        [Column(TypeName = "char")]
        [StringLength(3)]
        public string? Code { get; set; }
        [StringLength(80)]
        public string Name { get; set; }
        [StringLength(80)]
        public string NiceName { get; set; }
        public int? NumCode { get; set; }
        public int? PhoneCode { get; set; }
    }
}
