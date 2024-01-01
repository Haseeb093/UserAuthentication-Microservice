using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Domain.Models
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        [StringLength(2)]
        public char? IsoCode { get; set; }
        [StringLength(3)]
        public char? Code { get; set; } 
        public string Name { get; set; }
        public string NiceName { get; set; }
        public int? NumCode { get; set; }
        public int? PhoneCode { get; set; }
        public ICollection<States> States { get; set; }
    }
}
