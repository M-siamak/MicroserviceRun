using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Services
{
    public class MustOwnBasketRequirement : IAuthorizationRequirement
    {
    }
}
