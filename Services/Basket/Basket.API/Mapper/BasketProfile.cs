using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.API.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
            CreateMap<ProductItem, ShoppingCartItem > ().ReverseMap();
        }
    }
}
