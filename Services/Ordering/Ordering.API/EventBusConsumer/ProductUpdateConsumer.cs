using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit;
using Ordering.Application.Features.Oeders.Commands.CheckoutOrder;
using Ordering.Application.Features.Oeders.Commands.UpdateProduct;
using IMediator = MediatR.IMediator;

namespace Ordering.API.EventBusConsumer
{
    public class ProductUpdateConsumer : IConsumer<ProductUpdateEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ProductUpdateConsumer> _logger;

        public ProductUpdateConsumer(IMapper mapper, IMediator mediator, ILogger<ProductUpdateConsumer> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;

        }

        public async Task Consume(ConsumeContext<ProductUpdateEvent> context)
        {
            var command = _mapper.Map<UpdateProductCommand>(context.Message);

            await _mediator.Send(command);

            _logger.LogInformation("ProductEventConsumer consumed successfully. Update Product");

        }
    }
}
