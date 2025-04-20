using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Specifications
{
    /// <summary>
    /// Evaluator for specifications.
    /// </summary>
    public class SpecificationEvaluator : ISpecificationEvaluator
    {
        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The query.</returns>
        public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class, IEntity
        {
            var query = inputQuery;

            // Apply criteria
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply ordering
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            // Apply grouping
            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy)
                    .SelectMany(x => x);
            }

            // Apply includes
            query = specification.Includes.Aggregate(
                query,
                (current, include) => current.Include(include));

            // Apply include strings
            query = specification.IncludeStrings.Aggregate(
                query,
                (current, include) => current.Include(include));

            // Apply paging
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip!.Value)
                    .Take(specification.Take!.Value);
            }

            return query;
        }
    }
}