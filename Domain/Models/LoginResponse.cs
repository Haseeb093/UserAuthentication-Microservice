using System.IdentityModel.Tokens.Jwt;

namespace Domain.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
        public string UserName { get; set; }

    }
}
