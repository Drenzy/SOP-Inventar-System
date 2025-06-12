using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Archive_ItemGroup
    {
        [Key]
        public int Id { get; set; }
      
        [Column(TypeName = "datetime")]
        public DateTime DeleteTime { get; set; }

        [Column(TypeName = "tinyint")]
        public int ItemTypeId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ModelName { get; set; }

        [Column(TypeName = "tinyint")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Manufacturer { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string WarrantyPeriod { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ArchiveNote { get; set; }

        //public Archive_ItemType ItemType { get; set; }
    }
}
