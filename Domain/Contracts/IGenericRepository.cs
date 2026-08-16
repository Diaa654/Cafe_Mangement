using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity, Tkey> where TEntity : class
    {
        //Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey>? specifications = null);
        //IQueryable<TEntity> GetQueryWithSpec(ISpecifications<TEntity, Tkey> specifications);
        //Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specifications);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Tkey id);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task AddAsync(TEntity entity);
        //Task<int> CountAsync(ISpecifications<TEntity, Tkey>? specifications = null);
    }
}
