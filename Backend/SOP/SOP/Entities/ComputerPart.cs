using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.Entities
{
    public class ComputerPart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PartGroup.Id")]
        public int PartGroupId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string SerialNumber { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ModelNumber { get; set; }

        public PartGroup PartGroup { get; set; }
        public Computer_ComputerPart Computer_ComputerPart { get; set; }

    }
}
