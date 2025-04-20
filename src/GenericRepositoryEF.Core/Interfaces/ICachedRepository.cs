namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a cached repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ICachedRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Invalidates the cache.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvalidateCacheAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for a cached repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ICachedRepository<T, TKey> : ICachedRepository<T>, IRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Invalidates the cache for an entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvalidateCacheForEntityAsync(TKey id, CancellationToken cancellationToken = default);
    }
}