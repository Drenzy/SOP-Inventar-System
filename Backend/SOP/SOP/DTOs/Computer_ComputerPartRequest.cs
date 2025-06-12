using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class Computer_ComputerPartRequest
    {
        [Required]
        public int ComputerId { get; set; }

        [Required]
        public int ComputerPartId { get; set; }
    }
}
