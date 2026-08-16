using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : class
    {
        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> Includes { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        public ICollection<Func<IQueryable<TEntity>, IQueryable<TEntity>>> ComplexIncludes { get; } = [];
        protected void AddComplexInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression)
        {
            ComplexIncludes.Add(includeExpression);
        }
        #endregion

        #region Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        public Expression<Func<TEntity, object>> ThenBy { get; private set; }

        public Expression<Func<TEntity, object>> ThenByDescending { get; private set; }

        public void AddThenBy(Expression<Func<TEntity, object>> thenByExpression)
        {
            ThenBy = thenByExpression;
        }

        public void AddThenByDescending(Expression<Func<TEntity, object>> thenByDescExpression)
        {
            ThenByDescending = thenByDescExpression;
        }


        #endregion


        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public void ApplyPaging(int pageSize, int pageIndex)
        {
            IsPagingEnabled = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
        #endregion

    }
}
