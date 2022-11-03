using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly OrderContext _orderingContext;

        public ProductRepository(OrderContext orderingContext)
        {
            _orderingContext = orderingContext;
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            var result = await _orderingContext.Products.Where(o => o.ProductName == productName).ToListAsync();
            return result;
        }
        public async Task UpdateAsync(Product productToUpadate)
        {
            _orderingContext.Products.Update(productToUpadate);
            await _orderingContext.SaveChangesAsync();
        }

    }
}
