using GenericRepositoryEF.Core.Extensions;
using GenericRepositoryEF.Core.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Non-generic specification evaluator.
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
    /// Specification evaluator for applying specifications to queries.
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
        public virtual IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // Apply tracking
            if (!specification.IsTrackingEnabled)
            {
                query = query.AsNoTracking();
            }

            // Apply criteria
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply include strings
            foreach (var includeString in specification.IncludeStrings)
            {
                query = query.Include(includeString);
            }

            // Apply include expressions
            foreach (var includeExpression in specification.Includes)
            {
                query = query.Include(includeExpression);
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
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            // Apply paging
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip.GetValueOrDefault())
                             .Take(specification.Take.GetValueOrDefault());
            }

            return query;
        }
    }
}