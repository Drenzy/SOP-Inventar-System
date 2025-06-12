using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Role.Id")]
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

        public Role Role { get; set; }

        public List<Loan> Loans { get; set; }
    }
}
