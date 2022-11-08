using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Entities
{
    public class MustOwnBasketRequirement : IAuthorizationRequirement
    {
    }
}
