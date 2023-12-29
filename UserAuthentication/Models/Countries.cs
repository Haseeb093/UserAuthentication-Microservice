using System.Diagnostics;

namespace UserAuthentication.Models
{
    public class Countries
    {
        public int CountryId { get; set; }
        public char IsoCode { get; set; }
        public char Code { get; set; }
        public string Name { get; set; }
        public string NiceName { get; set; }
        public int NumCode { get; set; }
        public int PhoneCode { get; set; }
        public States States { get; set; }
    }
}
