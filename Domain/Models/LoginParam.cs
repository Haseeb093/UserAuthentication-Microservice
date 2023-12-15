using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LoginParam
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; } = string.Empty;
    }
}
