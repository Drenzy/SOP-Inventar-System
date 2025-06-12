using Azure.Core.Pipeline;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class StatusHistory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Item.Id")]
        public int ItemId { get; set; }

        [ForeignKey("Status.Id")]
        public int StatusId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StatusUpdateDate { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Note { get; set; }

        public Status Status { get; set; }

        public Item Item { get; set; }
    }
}
