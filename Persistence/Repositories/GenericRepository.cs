using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey>(CafeDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync()=> await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);
        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        #region Later
        //public async Task<int> CountAsync(ISpecifications<TEntity, TKey>? specifications = null)
        //{
        //    return await SpecificationsEvaluator.GetCountQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();
        //}

        //public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey>? specifications)
        //{
        //    return await SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();
        //}

        //public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        //{
        //    return await SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        //}


        //public IQueryable<TEntity> GetQueryWithSpec(ISpecifications<TEntity, TKey> specifications)
        //{
        //    return SpecificationsEvaluator.GetQuery(_dbContext.Set<TEntity>(), specifications);
        //} 
        #endregion

    }
}
