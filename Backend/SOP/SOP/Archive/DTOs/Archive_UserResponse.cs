using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.Archive.DTOs
{
    public class Archive_UserResponse
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Consider removing this for security reasons
        public bool TwoFactorAuthentication { get; set; }
        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }
    }
}
