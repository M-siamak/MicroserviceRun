using Basket.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Basket.API.Entities
{
    public class MustOwnBasketHandler : AuthorizationHandler<MustOwnBasketRequirement>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<MustOwnBasketHandler> _logger;

        public MustOwnBasketHandler(
            IBasketRepository basketRepository,
            ILogger<MustOwnBasketHandler> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, MustOwnBasketRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            if (filterContext == null)
            {
                context.Fail();
                return;
            }

            var imageId = filterContext.RouteData.Values["id"].ToString();
            if (!Guid.TryParse(imageId, out Guid imageIdAsGuid))
            {
                _logger.LogError($"`{imageId}` is not a Guid.");
                context.Fail();
                return;
            }

            var subClaim = context.User.Claims.FirstOrDefault(c => c.Type == "role");
            if (subClaim == null)
            {
                _logger.LogError($"User.Claims don't have the `role` claim.");
                context.Fail();
                return;
            }

            var ownerId = subClaim.Value;
            if (!await _basketRepository.IsImageOwnerAsync(imageIdAsGuid, ownerId))
            {
                _logger.LogError($"`{ownerId}` is not the owner of `{imageIdAsGuid}` image.");
                context.Fail();
                return;
            }

            // all checks out
            context.Succeed(requirement);
        }
    }
}
