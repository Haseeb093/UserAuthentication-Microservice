using System.IdentityModel.Tokens.Jwt;

namespace Domain.CustomModels
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
