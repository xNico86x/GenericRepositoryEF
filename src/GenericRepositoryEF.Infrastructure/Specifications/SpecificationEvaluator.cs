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
        /// Creates a query using the specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The query result.</returns>
        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class, IEntity
        {
            // Modify the query by applying the specification
            query = ApplySpecification(query, specification);

            // Return the modified query
            return query;
        }

        /// <summary>
        /// Applies a specification to a query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The modified query.</returns>
        protected virtual IQueryable<T> ApplySpecification<T>(IQueryable<T> query, ISpecification<T> specification) where T : class, IEntity
        {
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