using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class LoanResponse
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public LoanUserResponse LoanUser { get; set; }
        public LoanItemResponse LoanItem { get; set; }
    }

    public class LoanUserResponse 
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool TwoFactorAuthentication { get; set; }
    }

    public class LoanItemResponse 
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public int ItemGroupId { get; set; }

        public string SerialNumber { get; set; } = string.Empty;
    }
}
