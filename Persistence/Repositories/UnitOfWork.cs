using Domain.Contracts;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork(CafeDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var repo))
            {
                return (IGenericRepository<TEntity, TKey>)repo;
            }
            var repositoryInstance = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(EntityType, repositoryInstance);
            return repositoryInstance;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
