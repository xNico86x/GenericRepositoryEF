namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a repository that supports caching.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ICachedRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Invalidates all cache entries for the entity type.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvalidateCacheAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for a repository that supports caching with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ICachedRepository<T, TKey> : IRepository<T, TKey>, ICachedRepository<T>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Invalidates the cache entry for the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvalidateCacheForIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}