namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a read-only repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of entities.</returns>
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of entities.</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of entities.</returns>
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether any entity exists using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity exists, false otherwise.</returns>
        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity, or null if no entity exists.</returns>
        Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A single entity, or null if no entity exists.</returns>
        Task<T?> SingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for a read-only repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IReadOnlyRepository<T, TKey> : IReadOnlyRepository<T>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity, or null if the entity does not exist.</returns>
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}