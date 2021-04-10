using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
         Task<TEntity> GetByIdAsync(int id);
         Task<List<TEntity>> GetAllAsync();
         Task<TEntity> GetEntityWithSpec(ISpecification<TEntity> spec);
         Task<List<TEntity>> ListAsync(ISpecification<TEntity> spec);
         Task<int> CountAsync(ISpecification<TEntity> spec);
    }
}