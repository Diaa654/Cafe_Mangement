using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public ICollection<Expression<Func<TEntity, object>>> Includes { get; }
        public ICollection<Func<IQueryable<TEntity>, IQueryable<TEntity>>> ComplexIncludes { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        public Expression<Func<TEntity, object>> ThenBy { get; }
        public Expression<Func<TEntity, object>> ThenByDescending { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPagingEnabled { get; }
    }
}
