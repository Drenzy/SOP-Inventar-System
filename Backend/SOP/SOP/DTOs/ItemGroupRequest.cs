using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class ItemGroupRequest
    {

        [Required]
        public int ItemTypeId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The ModelNumber cannot be longer than 255 chars")]
        public string ModelName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The Manufacturer cannot be longer than 255 chars")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The ModelNumber cannot be longer than 255 chars")]
        public string WarrantyPeriod { get; set; }
    }
}
