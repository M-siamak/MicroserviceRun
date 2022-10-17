using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Queries.GetOrdersList
{
    public class GetOrderListsQuery : IRequest<List<OrdersDto>>
    {
        public string UserName { get; set; }
        public GetOrderListsQuery(string userName)
        {
            UserName = userName;
        }
    }
}
