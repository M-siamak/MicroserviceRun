using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Oeders.Commands.CheckoutOrder;
using Ordering.Application.Features.Oeders.Commands.DeleteOrder;
using Ordering.Application.Features.Oeders.Commands.UpdateOrder;
using Ordering.Application.Features.Oeders.Queries.GetOrdersList;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<OrdersDto>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrderListsQuery(userName);
            var orders = _mediator.Send(query);
            return Ok(orders);
        }


        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]

        public async Task<ActionResult<int>> CheckoutOrder(CheckoutOrderCommand command)
        {
            var result = _mediator.Send(command);
            return Ok(result);
        }


        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            _mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id:int}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
