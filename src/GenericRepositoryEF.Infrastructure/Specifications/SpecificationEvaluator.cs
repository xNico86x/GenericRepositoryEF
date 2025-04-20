using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Specifications
{
    /// <summary>
    /// Implementation of a specification evaluator.
    /// </summary>
    public class SpecificationEvaluator : ISpecificationEvaluator
    {
        /// <summary>
        /// Gets a typed specification evaluator.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The specification evaluator.</returns>
        public ISpecificationEvaluator<T> For<T>() where T : class, IEntity
        {
            return new SpecificationEvaluator<T>();
        }
    }

    /// <summary>
    /// Implementation of a typed specification evaluator.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class SpecificationEvaluator<T> : ISpecificationEvaluator<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets a query with a specification applied.
        /// </summary>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The query with the specification applied.</returns>
        public IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // Apply tracking
            if (!specification.IsTrackingEnabled)
            {
                query = query.AsNoTracking();
            }

            // Apply the criteria if it exists
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply the includes
            query = specification.Includes.Aggregate(query,
                (current, include) => current.Include(include));

            // Apply the include strings
            query = specification.IncludeStrings.Aggregate(query,
                (current, include) => current.Include(include));

            // Apply the order by
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Apply the group by
            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Apply the paging
            if (specification.IsPagingEnabled)
            {
                if (specification.Skip.HasValue && specification.Take.HasValue)
                {
                    query = query.Skip(specification.Skip.Value).Take(specification.Take.Value);
                }
                else if (specification.Take.HasValue)
                {
                    query = query.Take(specification.Take.Value);
                }
            }

            return query;
        }
    }
}