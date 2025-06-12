using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class Archive_RequestResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string RecipientEmail { get; set; } = string.Empty;

        public string Item { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime DeleteTime { get; set; }
        public string ArchiveNote { get; set; }

    }

}
