namespace UserAuthentication.Models
{
    public class Cities
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public ICollection<States> States { get; set; }
    }
}
