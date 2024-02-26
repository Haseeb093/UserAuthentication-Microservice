using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Rooms
    {
        [Key]
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomTypes RoomTypes { get; set; }

        [StringLength(50)]
        public string RoomName { get; set; }

        [StringLength(4)]
        [Column(TypeName = "char")]
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public int BlockId { get; set; }

        [ForeignKey("BlockId")]
        public Blocks Blocks { get; set; }
        public int FloorId { get; set; }

        [ForeignKey("FloorId")]
        public Floors Floors { get; set; }
        public Boolean Available { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
