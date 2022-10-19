using Shopping.Aggrigator.Models;

namespace Shopping.Aggrigator.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}
