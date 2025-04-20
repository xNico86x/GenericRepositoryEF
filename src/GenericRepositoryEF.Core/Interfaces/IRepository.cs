namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a range of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Updates a range of entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>The deleted entity.</returns>
        T Delete(T entity);

        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        void DeleteRange(IEnumerable<T> entities);
    }

    /// <summary>
    /// Interface for a repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IRepository<T, TKey> : IRepository<T>, IReadOnlyRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Deletes an entity from the repository by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}