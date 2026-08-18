namespace TanfeethIntegration.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Threading.Tasks;
    using TanfeethIntegration.Services;

    public class AdminAuthorizeAttribute : TypeFilterAttribute
    {
        public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizeFilter))
        {
        }

        private class AdminAuthorizeFilter : IAsyncAuthorizationFilter
        {
            private readonly UserService _userService;

            public AdminAuthorizeFilter(UserService userService)
            {
                _userService = userService;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                var currentUser = await _userService.GetCurrentUserAsync(user);
                if (currentUser == null || !currentUser.IsAdmin)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }

}
