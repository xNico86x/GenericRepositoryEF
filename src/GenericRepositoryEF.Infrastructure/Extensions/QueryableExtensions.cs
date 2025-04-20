using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies paging to the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query to page.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paged result.</returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query, 
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var totalItems = await query.CountAsync(cancellationToken);
            
            var skip = (pageNumber - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedResult<T>(items, totalItems, pageNumber, pageSize);
        }

        /// <summary>
        /// Applies dynamic ordering to the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query to order.</param>
        /// <param name="propertyName">The property name to order by.</param>
        /// <param name="direction">The order direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderByProperty<T>(
            this IQueryable<T> query, 
            string propertyName, 
            OrderByDirection direction = OrderByDirection.Ascending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(propertyName)
                ?? throw new ArgumentException($"Property {propertyName} not found on type {entityType.Name}");

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = direction == OrderByDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable), 
                methodName, 
                new[] { entityType, property.PropertyType },
                query.Expression, 
                Expression.Quote(lambda));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(resultExpression);
        }

        /// <summary>
        /// Applies a second-level ordering to an already ordered query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query to order.</param>
        /// <param name="propertyName">The property name to order by.</param>
        /// <param name="direction">The order direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> ThenByProperty<T>(
            this IOrderedQueryable<T> query, 
            string propertyName, 
            OrderByDirection direction = OrderByDirection.Ascending)
        {
            var entityType = typeof(T);
            var property = entityType.GetProperty(propertyName)
                ?? throw new ArgumentException($"Property {propertyName} not found on type {entityType.Name}");

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var methodName = direction == OrderByDirection.Ascending ? "ThenBy" : "ThenByDescending";
            var resultExpression = Expression.Call(
                typeof(Queryable), 
                methodName, 
                new[] { entityType, property.PropertyType },
                query.Expression, 
                Expression.Quote(lambda));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
