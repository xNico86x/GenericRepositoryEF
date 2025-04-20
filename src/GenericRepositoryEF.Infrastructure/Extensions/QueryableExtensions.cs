using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies paging to the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged query.</returns>
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Creates a paged result from the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The paged result.</returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return PagedResult<T>.Empty(pageNumber, pageSize);
            }

            var items = await query.ApplyPaging(pageNumber, pageSize).ToListAsync(cancellationToken);
            return PagedResult<T>.Create(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Applies ordering to the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> ApplyOrdering<T, TKey>(this IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, TKey>> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Applies ordering to the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> ThenApplyOrdering<T, TKey>(this IOrderedQueryable<T> query, System.Linq.Expressions.Expression<Func<T, TKey>> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? query.ThenBy(keySelector)
                : query.ThenByDescending(keySelector);
        }
    }
}