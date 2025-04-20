using System.Linq.Expressions;
using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="IQueryable{T}"/> interface.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Creates a paged result.
        /// </summary>
        /// <typeparam name="T">The type of elements in the query.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged result.</returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Orders the query using the provided expression.
        /// </summary>
        /// <typeparam name="T">The type of elements in the query.</typeparam>
        /// <typeparam name="TKey">The type of the key used for ordering.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="direction">The order by direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Orders the query using the provided expression, after a previous ordering.
        /// </summary>
        /// <typeparam name="T">The type of elements in the query.</typeparam>
        /// <typeparam name="TKey">The type of the key used for ordering.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="direction">The order by direction.</param>
        /// <returns>The ordered query.</returns>
        public static IOrderedQueryable<T> ThenBy<T, TKey>(this IOrderedQueryable<T> query, Expression<Func<T, TKey>> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                ? query.ThenBy(keySelector)
                : query.ThenByDescending(keySelector);
        }
    }
}