using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistance;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        public CheckoutOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService email, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _emailService = email;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _repository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is successfuly created.");

            await SendMail(newOrder);

            return newOrder.Id;
        }





        private async Task<bool> SendMail(Order newOrder)
        {
            var email = new Email()
            {
                To = "plamenpentchev@yahoo.com",
                Subject = "New Order has been created.",
                Body = "New order has just been created."
            };
            try
            {
                return await _emailService.SendEmail(email);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Order failed due to an error in the mail service. {ex.Message}");
                return false;
            }
        }
    }
}
