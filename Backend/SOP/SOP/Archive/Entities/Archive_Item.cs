using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SOP.Archive.Entities
{
    public class Archive_Item
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [Column(TypeName = "tinyint")]
        public int RoomId { get; set; }

        [Column(TypeName = "tinyint")]
        public int ItemGroupId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string SerialNumber { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }

        public List<Archive_StatusHistory>? StatusHistories { get; set; }

    }
}
