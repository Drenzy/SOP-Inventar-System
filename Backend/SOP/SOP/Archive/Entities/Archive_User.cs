using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Archive.Entities
{
    public class Archive_User
    {
        [Key]
        public int Id { get; set; }
      
        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [Column(TypeName = "tinyint")]
        public int RoleId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? Password { get; set; }

        [Column(TypeName = "bit")]
        public bool TwoFactorAuthentication { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string? TwoFactorSecretKey { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }
    }
}
