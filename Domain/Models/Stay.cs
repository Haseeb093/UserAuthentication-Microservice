using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Stay
    {
        [Key]
        public int StayId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Rooms Rooms { get; set; }
        public string PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Users Patients { get; set; }
        public DateOnly AdmitionDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public TimeOnly AdmitionTime { get; set; }
        public TimeOnly CheckOutTime { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
