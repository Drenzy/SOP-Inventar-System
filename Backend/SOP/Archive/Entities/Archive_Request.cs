using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Archive_Request
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [Column(TypeName = "tinyint")]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string RecipientEmail { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Item { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Message { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Status { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }

    }
}
