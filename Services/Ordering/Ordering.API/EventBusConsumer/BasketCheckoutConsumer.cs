using AutoMapper;
using MediatR;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.Features.Oeders.Commands.CheckoutOrder;
using IMediator = MediatR.IMediator;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        
        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);

            //_logger.LogInformation("BasketCheckoutEvent consumed successfully. Created order Id: {0}", result);

        }
    } 
}
