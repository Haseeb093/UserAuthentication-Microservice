﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace UserAuthentication.Models
{
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        public char IsoCode { get; set; }
        public char Code { get; set; }
        public string Name { get; set; }
        public string NiceName { get; set; }
        public int NumCode { get; set; }
        public int PhoneCode { get; set; }
        public ICollection<States> States { get; set; }
    }
}
