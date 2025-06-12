using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOP.Entities
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User.Id")]
        public int UserId { get; set; }

        [ForeignKey("Item.Id")]
        public int ItemId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LoanDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ReturnDate { get; set; }

        public Item Item { get; set; }

        public User User { get; set; }
    }
}
