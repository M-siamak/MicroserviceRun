using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Oeders.Commands.CheckoutOrder;
using Ordering.Application.Features.Oeders.Commands.UpdateProduct;
using Ordering.Domain.Entities;

namespace Ordering.API.Mapping
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
            CreateMap<ProductItem,Product>().ReverseMap();
            CreateMap<UpdateProductCommand , ProductUpdateEvent>().ReverseMap();
        }
    }
}
