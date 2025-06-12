using SOP.Entities;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class ComputerPartRequest
    {
        [Required]
        public int PartGroupId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The SerialNumber cannot be longer than 50 chars")]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The ModelNumber cannot be longer than 50 chars")]
        public string ModelNumber { get; set; }

    }
}
