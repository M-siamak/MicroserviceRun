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

namespace Ordering.Application.Features.Oeders.Commands.UpdateProduct
{
    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IProductRepository repository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productToUpadate = await _repository.GetProductByName(request.ProductName);

            if (productToUpadate == null)
            {
                _logger.LogError("Product not exist on database.");
                throw new NotFoundException(nameof(Order), request.ProductName);
            }
            foreach(var item in productToUpadate)
            {
                item.ProductName = request.NewProductName;
                await _repository.UpdateAsync(item);
            }
            

            _logger.LogInformation($"Product is succesfuly Updated.");

            return Unit.Value;

        }
    }
    }
