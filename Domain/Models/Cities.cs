using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Cities
    {
        [Key]
        public int CityId { get; set; }
        [ForeignKey("State_Id")]
        public States States { get; set; }
        public string Name { get; set; }
    }
}
