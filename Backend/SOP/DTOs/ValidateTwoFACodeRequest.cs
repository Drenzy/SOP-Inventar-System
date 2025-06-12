namespace SOP.DTOs
{
    public class ValidateTwoFACodeRequest
    {
        public string Email { get; set; }
        public string TwoFactorAuthenticationCode { get; set; }
    }
}
