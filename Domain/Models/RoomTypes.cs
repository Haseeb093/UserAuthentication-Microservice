using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomTypes
    {
        [Key]
        public int RoomTypeId { get; set; }

        [StringLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
