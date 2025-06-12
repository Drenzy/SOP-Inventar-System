namespace SOP.DTOs
{
    public class SignInRequest
    {

        [Required]
        [StringLength(40, ErrorMessage = "Password cannot be longer than 40 chars")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(40, ErrorMessage = "Email cannot be longer than 40 chars")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
