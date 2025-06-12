using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Computer
    {
        [Key]
        [ForeignKey("Item")]
        public int Id { get; set; }

        public Item Item { get; set; }

        public List<Computer_ComputerPart> Computer_ComputerParts { get; set; }
    }
}
