using AutoMapper;
using MediatR;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.Features.Oeders.Commands.CheckoutOrder;
using IMediator = MediatR.IMediator;
using Ordering.Domain.Entities;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger;
        
        public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;   
            
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
             var eventMessage = _mapper.Map<BasketCheckoutEvent>(context.Message);

            var command = _mapper.Map<CheckoutOrderCommand>(eventMessage);

            var result = await _mediator.Send(command); 

            _logger.LogInformation("BasketCheckoutEvent consumed successfully. Created order Id: {0}", result);

        }
    } 
}
