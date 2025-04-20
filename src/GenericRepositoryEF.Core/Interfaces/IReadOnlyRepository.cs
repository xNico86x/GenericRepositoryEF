using GenericRepositoryEF.Core.Models;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a read-only repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Finds an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <returns>The found entity or null.</returns>
        T? Find(object id);

        /// <summary>
        /// Finds an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        Task<T?> FindAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        IReadOnlyList<T> GetAll();

        /// <summary>
        /// Gets all entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entities.</returns>
        IReadOnlyList<T> GetAll(ISpecification<T> specification);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entity or null.</returns>
        T? GetSingleOrDefault(ISpecification<T> specification);

        /// <summary>
        /// Gets a single entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        Task<T?> GetSingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entity or null.</returns>
        T? GetFirstOrDefault(ISpecification<T> specification);

        /// <summary>
        /// Gets the first entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        Task<T?> GetFirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of entities.
        /// </summary>
        /// <returns>The number of entities.</returns>
        int Count();

        /// <summary>
        /// Counts the number of entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The number of entities.</returns>
        int Count(ISpecification<T> specification);

        /// <summary>
        /// Counts the number of entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities.</returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities.</returns>
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity with the specification exists.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if an entity exists, otherwise false.</returns>
        bool Any(ISpecification<T> specification);

        /// <summary>
        /// Checks if an entity with the specification exists.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if an entity exists, otherwise false.</returns>
        Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged result.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged result.</returns>
        PagedResult<T> GetPaged(ISpecification<T> specification, int pageNumber, int pageSize);

        /// <summary>
        /// Gets a paged result.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The paged result.</returns>
        Task<PagedResult<T>> GetPagedAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}