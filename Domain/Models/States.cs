using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }

        [ForeignKey("CountryId")]
        public Countries Countries { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(3)]
        [Column(TypeName = "char")]
        public string Code { get; set; }
    }
}
