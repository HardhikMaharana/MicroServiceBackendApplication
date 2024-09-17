using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models
{
    public class Users:IdentityUser
    {

        public string Password { get; set; } = string.Empty;
        public string RefteshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
