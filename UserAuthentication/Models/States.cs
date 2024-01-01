﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAuthentication.Models
{
    public class States
    {
        [Key]
        public int StateId { get; set; }
        [ForeignKey("Country_Id")]
        public Countries CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Cities> Cities { get; set; }
    }
}
