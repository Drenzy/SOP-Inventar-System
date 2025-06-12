namespace SOP.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _users;

        // Constructor to initialize the list of authorized users
        public AuthorizeAttribute(params string[] users)
        {
            _users = users ?? Array.Empty<string>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action has [AllowAnonymous] attribute
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            // Get the currently logged-in user from the context
            UserResponse user = (UserResponse)context.HttpContext.Items["User"];

            // Check if the user is not logged in or does not have the required role
            if (user == null || (_users.Any() && !_users.Contains(user.UserRole.Name)))
            {
                // Return unauthorized response if user is not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
