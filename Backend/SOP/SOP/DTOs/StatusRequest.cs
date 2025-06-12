using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class StatusRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "The Name cannot be longer than 255 chars")]
        public string Name { get; set; }
    }
}
