﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Genders
    {
        [Key]
        public int GenderId { get; set; }

        [StringLength(10)]
        public string Name { get; set; }

    }
}
