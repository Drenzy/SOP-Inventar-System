using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class PartGroupRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "The PartName cannot be longer than 255 chars")]
        public string PartName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The Manufacturer cannot be longer than 255 chars")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The WarrantyPeriod cannot be longer than 255 chars")]
        public string WarrantyPeriod { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int PartTypeId { get; set; }
    }
}
