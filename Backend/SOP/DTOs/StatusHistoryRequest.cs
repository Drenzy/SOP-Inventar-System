using SOP.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class StatusHistoryRequest
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int StatusId { get; set; }

        //[Required]
        public DateTime StatusUpdateDate { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "The Note cannot be longer than 255 chars")]
        public string Note { get; set; }
    }
}
