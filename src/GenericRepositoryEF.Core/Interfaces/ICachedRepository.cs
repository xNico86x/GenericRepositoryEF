using GenericRepositoryEF.Core.Models;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a cached repository that adds caching support to a standard repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ICachedRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        bool RefreshCache(object id);

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        Task<bool> RefreshCacheAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Refreshes the cache for a collection of entities.
        /// </summary>
        /// <param name="specification">The specification to refresh.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        bool RefreshCache(ISpecification<T> specification);

        /// <summary>
        /// Refreshes the cache for a collection of entities.
        /// </summary>
        /// <param name="specification">The specification to refresh.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        Task<bool> RefreshCacheAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Refreshes the entire cache for the entity type.
        /// </summary>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        bool RefreshEntireCache();

        /// <summary>
        /// Refreshes the entire cache for the entity type.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        Task<bool> RefreshEntireCacheAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        bool RemoveFromCache(object id);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        Task<bool> RemoveFromCacheAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Clears the entire cache for the entity type.
        /// </summary>
        /// <returns>True if the cache was cleared, otherwise false.</returns>
        bool ClearCache();

        /// <summary>
        /// Clears the entire cache for the entity type.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was cleared, otherwise false.</returns>
        Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default);
    }
}