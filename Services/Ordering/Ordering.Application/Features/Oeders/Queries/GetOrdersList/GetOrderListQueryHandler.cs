using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Queries.GetOrdersList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListsQuery, List<OrdersDto>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderListQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrdersDto>> Handle(GetOrderListsQuery request, CancellationToken cancellationToken)
        {
            var orderList =  await _repository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrdersDto>>(orderList);
        }
    }
}
