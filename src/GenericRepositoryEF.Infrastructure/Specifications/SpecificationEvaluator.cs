using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Specifications
{
    /// <summary>
    /// Evaluates specifications to build Entity Framework queries.
    /// </summary>
    public class SpecificationEvaluator : ISpecificationEvaluator
    {
        /// <summary>
        /// Gets the query that represents the specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>The resulting query.</returns>
        public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
        {
            var query = inputQuery;

            // Apply AsNoTracking if specified
            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Apply filtering
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Include each simple property
            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }

            // Include each string-based path
            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            // Include each complex nested property using grouped includes
            foreach (var (selector, navigationPropertyPath) in specification.GroupedIncludes)
            {
                query = query.Include(selector)
                             .ThenInclude(navigationPropertyPath);
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