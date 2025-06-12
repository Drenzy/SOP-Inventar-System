using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class RequestResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string RecipientEmail { get; set; } = string.Empty;

        public string Item { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string Status { get; set; } = string.Empty;

        public RequestUserResponse RequestUser { get; set; }
    }

    public class RequestUserResponse
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool TwoFactorAuthentication { get; set; }
    }
}
