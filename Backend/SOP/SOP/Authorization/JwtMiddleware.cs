namespace SOP.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor to initialize the next middleware in the pipeline
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            // Retrieve the JWT token from the Authorization header
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            // Validate the JWT token and get the user ID if valid
            int? userId = jwtUtils.ValidateJwtToken(token);
            
            if (userId != null)
            {
                // Attach user data to the context if the token is valid
                var user = await userRepository.FindByIdAsync(userId.Value);
                context.Items["User"] = UserController.MapUserToUserResponsePublic(user);
            }

            // Pass control to the next middleware
            await _next(context);
        }
    }
}
