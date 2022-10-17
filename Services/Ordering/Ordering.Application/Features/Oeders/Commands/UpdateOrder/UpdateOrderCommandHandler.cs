using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpadate = await _repository.GetByIdAsync(request.Id);

            if(orderToUpadate == null)
            {
                _logger.LogError("Order not exist on database.");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, orderToUpadate, typeof(UpdateOrderCommand), typeof(Order));

            await _repository.UpdateAsync(orderToUpadate);

            _logger.LogInformation($"Order {orderToUpadate.Id} is succesfuly Updated.");

            return Unit.Value;

        }
    }
}
