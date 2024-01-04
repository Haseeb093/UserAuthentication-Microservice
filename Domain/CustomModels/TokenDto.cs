using System.IdentityModel.Tokens.Jwt;

namespace Domain.CustomModels
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
