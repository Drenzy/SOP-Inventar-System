using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class UserRequest
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required]
        public bool TwoFactorAuthentication { get; set; }
    }
}
