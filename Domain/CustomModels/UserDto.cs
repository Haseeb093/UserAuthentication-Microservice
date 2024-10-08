﻿using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.CustomModels
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int GenderId { get; set; }
        public string Gender { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string DateofBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string Role { get; set; }

    }
}
