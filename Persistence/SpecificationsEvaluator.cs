using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery,
             ISpecifications<TEntity, TKey>? specifications) where TEntity : class
        {
            var query = inputQuery;
            if (specifications != null)
            {
                if (specifications.Criteria != null)
                {
                    query = query.Where(specifications.Criteria);
                }
                if (specifications.Includes != null && specifications.Includes.Any())
                {
                    query = specifications.Includes.Aggregate(query, (current, include) => current.Include(include));
                }
                if (specifications.ComplexIncludes != null && specifications.ComplexIncludes.Any())
                {
                    query = specifications.ComplexIncludes.Aggregate(query, (current, include) => include(current));
                }
                if (specifications.OrderBy != null)
                {

                    query = query.OrderBy(specifications.OrderBy);

                    if (specifications.ThenBy is not null)
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenBy(specifications.ThenBy);
                    }
                    else if (specifications.ThenByDescending is not null)
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenByDescending(specifications.ThenByDescending);
                    }


                }
                else if (specifications.OrderByDescending != null)
                {
                    query = query.OrderByDescending(specifications.OrderByDescending);
                    if (specifications.ThenBy is not null)
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenBy(specifications.ThenBy);
                    }
                    else if (specifications.ThenByDescending is not null)
                    {
                        query = ((IOrderedQueryable<TEntity>)query).ThenByDescending(specifications.ThenByDescending);
                    }
                }
                if (specifications.IsPagingEnabled)
                {
                    query = query.Skip(specifications.Skip).Take(specifications.Take);
                }

            }
            return query;
        }
        public static IQueryable<TEntity> GetCountQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey>? spec) where TEntity : class
        {
            var query = inputQuery;

            if (spec != null && spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            return query;
        }
    }
}
