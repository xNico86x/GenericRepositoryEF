namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a cached repository that adds caching support to a standard repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ICachedRepository<T, TKey> : IRepository<T, TKey>, ICachedRepository<T>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        bool RefreshCache(TKey id);

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        Task<bool> RefreshCacheAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        bool RemoveFromCache(TKey id);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        Task<bool> RemoveFromCacheAsync(TKey id, CancellationToken cancellationToken = default);
    }
}