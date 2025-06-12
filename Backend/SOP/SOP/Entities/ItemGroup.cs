using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class ItemGroup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemType.Id")]
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

        public ItemType ItemType { get; set; }
    }
}
