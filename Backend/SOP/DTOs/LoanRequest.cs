using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class LoanRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }
    }
}
