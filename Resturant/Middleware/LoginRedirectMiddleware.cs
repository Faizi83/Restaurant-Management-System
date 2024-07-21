using System.Security.Claims;

namespace Resturant.Middleware
{
    public class LoginRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the user is authenticated
            if (!context.User.Identity.IsAuthenticated)
            {
                // Check if the current path is not login or register
                var path = context.Request.Path.Value.ToLower();
                if (!path.Contains("login") && !path.Contains("register"))
                {
                    // Redirect to login page
                    context.Response.Redirect("/login");
                    return;
                }
            }
            else
            {
                // Check if the user is not an Admin and is trying to access restricted pages
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
                var path = context.Request.Path.Value.ToLower();
                var restrictedPaths = new[] { "activeitems", "additems", "booktable", "updateitems" };

                if (role != "Admin" && restrictedPaths.Any(p => path.Contains(p)))
                {
                    // Redirect to unauthorized page or home page
                    context.Response.Redirect("/unauthorized");
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
