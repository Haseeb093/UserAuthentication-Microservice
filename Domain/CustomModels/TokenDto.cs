using System.IdentityModel.Tokens.Jwt;

namespace Domain.CustomModels
{
    public class TokenDto
    {
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
