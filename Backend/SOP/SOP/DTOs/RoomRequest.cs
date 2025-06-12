using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.DTOs
{
    public class RoomRequest
    {
        [Required]
        public int BuildingId { get; set; }

        [Required]
        public int RoomNumber { get; set; }
    }
}
