using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Specifications
{
    /// <summary>
    /// Evaluates specifications by applying them to IQueryable.
    /// </summary>
    public class SpecificationEvaluator<T> where T : class
    {
        /// <summary>
        /// Gets the query with the specification applied.
        /// </summary>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>The query with the specification applied.</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // Apply tracking options
            if (specification.AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            // Apply filtering
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            // Apply includes
            query = specification.Includes.Aggregate(query,
                (current, include) => current.Include(include));

            // Apply string-based include statements
            query = specification.IncludeStrings.Aggregate(query,
                (current, include) => current.Include(include));
                
            // Apply grouped includes (ThenInclude) 
            // Since we can't directly use ThenInclude with string paths, we'll use a simpler approach
            // for string-based navigation paths
            foreach (var (selector, navigationPropertyPath) in specification.GroupedIncludes)
            {
                // Create a combined path string instead of using ThenInclude
                var fullPath = $"{GetPropertyName(selector)}.{navigationPropertyPath}";
                query = query.Include(fullPath);
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
                query = query.Skip(specification.Skip.GetValueOrDefault())
                             .Take(specification.Take.GetValueOrDefault());
            }

            return query;
        }
        
        /// <summary>
        /// Extracts the property name from an expression.
        /// </summary>
        /// <param name="expression">The expression to extract the property name from.</param>
        /// <returns>The extracted property name.</returns>
        private static string GetPropertyName(Expression<Func<T, object>> expression)
        {
            if (expression == null)
                return string.Empty;
            
            // Handle different expression types to extract the property name
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (expression.Body is UnaryExpression unaryExpression && 
                    unaryExpression.Operand is MemberExpression memberExp)
            {
                return memberExp.Member.Name;
            }
            
            // For more complex expressions, try to extract from the expression body string
            var bodyString = expression.Body.ToString();
            if (bodyString.Contains('.'))
            {
                // Simple but effective way to handle basic cases - take the part after the last dot
                return bodyString.Split('.').Last().TrimEnd(')');
            }
            
            return string.Empty;
        }
    }
}
