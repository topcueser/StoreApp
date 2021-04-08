using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
         Task<Product> GetProductsByIdAsync(int id);
         Task<List<Product>> GetProductsAsync();
    }
}