namespace UserAuthentication.Models
{
    public class States
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Countries> Countries { get; set; }
    }
}
