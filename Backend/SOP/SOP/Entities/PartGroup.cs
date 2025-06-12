using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class PartGroup
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string PartName { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Manufacturer { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string WarrantyPeriod { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ReleaseDate { get; set; }

        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [ForeignKey("PartType.Id")]
        public int PartTypeId {get; set;}

        public PartType PartType { get; set;}
    }
}
