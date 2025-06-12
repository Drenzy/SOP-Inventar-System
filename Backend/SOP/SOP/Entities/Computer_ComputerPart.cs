using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Computer_ComputerPart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Computer.Id")]
        public int ComputerId { get; set; }

        [ForeignKey("ComputerPart.Id")]
        public int ComputerPartId { get; set; }

        public Computer Computer { get; set; }

        public ComputerPart ComputerPart { get; set; }
    }
}
