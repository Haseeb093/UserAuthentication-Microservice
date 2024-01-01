using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }
        [ForeignKey("Country_Id")]
        public Countries Countries { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }=string.Empty;
        public ICollection<Cities> Cities { get; set; }
    }
}
