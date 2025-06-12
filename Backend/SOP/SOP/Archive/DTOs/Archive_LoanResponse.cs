using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.Archive.DTOs
{
    public class Archive_LoanResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }
    }
}
