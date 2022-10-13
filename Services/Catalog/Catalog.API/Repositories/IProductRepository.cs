using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        #region READ
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task<IEnumerable<Product>> GetProductByCategory(string category);
        #endregion Read

        #region CREATE
        Task CreateProduct(Product product);
        #endregion CREATE

        #region UPDATE
        Task<bool> UpdateProduct(Product product);
        #endregion UPDATE

        #region DELETE
        Task<bool> DeleteProduct(string id);
        #endregion DELETE

    }
}
