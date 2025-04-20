using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Specifications;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a read-only repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All entities.</returns>
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities that match the specification.</returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity that matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity that matches the specification or throws an exception.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        /// <exception cref="EntityNotFoundException">If no entity matches the specification.</exception>
        Task<T> FirstOrDefaultAsync(ISpecification<T> specification, bool throwIfNotFound, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of entities that match the specification.</returns>
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any entity matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity matches the specification.</returns>
        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for a read-only repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IReadOnlyRepository<T, TKey> : IReadOnlyRepository<T>
        where T : class, IEntityWithKey<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets an entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity by identifier or throws an exception.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        /// <exception cref="EntityNotFoundException">If the entity is not found.</exception>
        Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default);
    }
}