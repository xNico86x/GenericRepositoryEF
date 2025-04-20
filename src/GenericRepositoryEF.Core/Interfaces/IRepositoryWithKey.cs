namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a repository with a typed key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IRepository<T, TKey> : IRepository<T>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity or null.</returns>
        T? GetById(TKey id);

        /// <summary>
        /// Gets an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity by ID or throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity.</returns>
        T GetByIdOrThrow(TKey id);

        /// <summary>
        /// Gets an entity by ID asynchronously or throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        void Delete(TKey id);

        /// <summary>
        /// Deletes an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    }
}