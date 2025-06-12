namespace SOP.DTOs
{
    public class SignInResponse
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;

        public Role Role { get; set; }
    }
}
