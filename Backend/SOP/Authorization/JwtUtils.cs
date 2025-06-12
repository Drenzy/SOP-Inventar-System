namespace SOP.Authorization
{
    public interface IJwtUtils
    {
        // Method to generate JWT token for a user
        public string GenerateJwtToken(User user);

        // Method to validate a JWT token and return user ID
        public int? ValidateJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;

        // Constructor to inject configuration (e.g., secret key from appsettings)
        public JwtUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            // Create a token handler to generate the JWT
            JwtSecurityTokenHandler tokenHandler = new();

            // Get the secret key from appsettings
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("Secret").Value);

            // Define the token descriptor with user claims and expiration time
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create and return the token as a string
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            // Return null if token is null
            if (token == null)
            {
                return null;
            }

            // Create a token handler for validation
            JwtSecurityTokenHandler tokenHandler = new();

            // Get the secret key from appsettings for validation
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("Secret").Value);

            try
            {
                // Validate the token with the secret key and settings
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Tokens expire exactly at expiration time
                }, out SecurityToken validatedToken);

                // Extract and return the user ID from the validated token
                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
                return userId;
            }
            catch (Exception)
            {
                // Return null if validation fails
                return null;
            }
        }
    }
}