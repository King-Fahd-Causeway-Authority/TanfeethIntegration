namespace TanfeethIntegration.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Threading.Tasks;
    using TanfeethIntegration.Services;

    public class UserExistsAuthorizeAttribute : TypeFilterAttribute
    {
        public UserExistsAuthorizeAttribute() : base(typeof(UserExistsAuthorizeFilter))
        {
        }

        private class UserExistsAuthorizeFilter : IAsyncAuthorizationFilter
        {
            private readonly UserService _userService;

            public UserExistsAuthorizeFilter(UserService userService)
            {
                _userService = userService;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new ChallengeResult();
                    return;
                }

                var currentUser = await _userService.GetCurrentUserAsync(user);
                if (currentUser == null)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }

}
