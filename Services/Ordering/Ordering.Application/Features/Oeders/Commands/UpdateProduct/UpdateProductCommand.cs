using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Oeders.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public string ProductName { get; set; }
        public string NewProductName { get; set; }
    }
}
