using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SOP.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
        public bool TwoFactorAuthentication { get; set; }
        public UserRoleResponse UserRole { get; set; }
        public List<UserLoanResponse> UserLoans { get; set; }
    }

    public class UserRoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UserLoanResponse
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public UserLoanItemResponse UserLoanItem { get; set; }
    }

    public class UserLoanItemResponse
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ItemGroupId { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public UserLoanItemItemGroupResponse UserLoanItemItemGroup { get; set; }
    }

    public class UserLoanItemItemGroupResponse
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string WarrantyPeriod { get; set; } = string.Empty;
    }
}
