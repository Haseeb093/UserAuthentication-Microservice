﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Floors
    {
        [Key]
        public int FloorId { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
  
    }
}
