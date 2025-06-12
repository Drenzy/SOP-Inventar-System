using Azure.Core.Pipeline;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Archive_StatusHistory
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [ForeignKey("Item.Id")]
        public int ItemId { get; set; }

        [Column(TypeName = "tinyint")]
        public int StatusId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StatusUpdateDate { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Note { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }

        public Archive_Item Item { get; set; }
    }
}
