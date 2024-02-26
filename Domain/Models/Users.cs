using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Users : IdentityUser
    {
        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string LastName { get; set; }
        public int GenderId { get; set; }

        [ForeignKey("GenderId")]
        public Genders Genders { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Departments Departments { get; set; }
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Countries Countries { get; set; }
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public States States { get; set; }
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public Cities Cities { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [StringLength(15)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string SecondaryPhoneNumber { get; set; }
        public string InsertedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
