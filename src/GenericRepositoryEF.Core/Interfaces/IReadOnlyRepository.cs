using GenericRepositoryEF.Core.Models;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a read-only repository for accessing entities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public interface IReadOnlyRepository<T, TKey> where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity, or null if not found.</returns>
        Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A read-only list of entities.</returns>
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets entities that satisfy the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A read-only list of filtered entities.</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Determines whether any entity satisfies the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity satisfies the predicate; otherwise, false.</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Counts the number of entities that satisfy the specified specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities, or null to count all entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities.</returns>
        Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a page of entities that satisfy the specified specification.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="specification">The specification to filter entities, or null to page all entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A paged result of entities.</returns>
        Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets entities that satisfy the specified specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A read-only list of filtered entities.</returns>
        Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a single entity that satisfies the specified specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity, or null if not found.</returns>
        Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Defines a read-only repository for accessing entities with an integer identifier.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IReadOnlyRepository<T> : IReadOnlyRepository<T, int> where T : class, IEntity<int>
    {
    }
}
