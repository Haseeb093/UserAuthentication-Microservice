﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Cities
    {
        [Key]
        public int CityId { get; set; }

        [ForeignKey("StateId")]
        public States States { get; set; }

        [StringLength(80)]
        public string Name { get; set; }
    }
}
