using Shopping.Aggrigator.Models;

namespace Shopping.Aggrigator.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
