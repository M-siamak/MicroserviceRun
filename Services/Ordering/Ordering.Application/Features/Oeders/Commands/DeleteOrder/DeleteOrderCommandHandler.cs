using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Oeders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository repository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var deleteOrder = await _repository.GetByIdAsync(request.Id);

            if(deleteOrder == null)
            {
                _logger.LogError("Order not exist on database.");
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await _repository.DeleteAsync(deleteOrder);

            _logger.LogInformation($"Order {deleteOrder.Id} is succesfuly Deleted.");

            return Unit.Value;
        }
    }
}
