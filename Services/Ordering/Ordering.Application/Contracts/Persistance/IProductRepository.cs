using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistance
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetProductByName(string productName);
        public Task UpdateAsync(Product productToUpadate);
    }
}
