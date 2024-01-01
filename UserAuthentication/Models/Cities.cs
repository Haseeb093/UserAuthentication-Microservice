﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    public class Cities
    {
        [Key]
        public int CityId { get; set; }
        [ForeignKey("State_Id")]
        public States StateId { get; set; }
        public string Name { get; set; }
    }
}
