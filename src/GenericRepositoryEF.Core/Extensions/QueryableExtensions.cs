using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Extensions
{
    /// <summary>
    /// Extension methods for IQueryable.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Tracks the query results or returns them as no-tracking.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="track">Whether to track the entities.</param>
        /// <returns>The query with tracking set.</returns>
        public static IQueryable<T> AsNoTracking<T>(this IQueryable<T> query) where T : class
        {
            // This is a simplified version for the core project that doesn't depend on EF Core
            // The actual implementation is in the Infrastructure project
            return query;
        }

        /// <summary>
        /// Includes a related entity in the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="path">The include path.</param>
        /// <returns>The query with the include.</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> query, string path) where T : class
        {
            // This is a simplified version for the core project that doesn't depend on EF Core
            // The actual implementation is in the Infrastructure project
            return query;
        }

        /// <summary>
        /// Includes a related entity in the query.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="path">The include path.</param>
        /// <returns>The query with the include.</returns>
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> path) where T : class
        {
            // This is a simplified version for the core project that doesn't depend on EF Core
            // The actual implementation is in the Infrastructure project
            return query;
        }
    }
}