using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.CompilerServices;

namespace Basket.API.Services
{
    public class MustOwnBasketHandler : AuthorizationHandler<MustOwnBasketRequirement>
    {

        private readonly ILogger<MustOwnBasketHandler> _logger;
        public MustOwnBasketHandler(ILogger<MustOwnBasketHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnBasketRequirement requirement)
        {

            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return;
            }
            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == "role");
            if (roleClaim == null)
            {
                _logger.LogError($"User.Claims don't have the `sub` claim.");
                context.Fail();
                return;
            }

            var userName = context.User.ToString();
            var ussssr = context.User.Claims.FirstOrDefault(p => p.Type == "name");

            context.Succeed(requirement);
        }
    }
}
