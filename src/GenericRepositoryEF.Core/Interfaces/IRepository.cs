using GenericRepositoryEF.Core.Models;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a repository for accessing and manipulating entities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public interface IRepository<T, TKey> : IReadOnlyRepository<T, TKey> where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Adds a range of new entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);
        
        /// <summary>
        /// Updates a range of existing entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        void UpdateRange(IEnumerable<T> entities);
        
        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);
        
        /// <summary>
        /// Deletes a range of entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        void DeleteRange(IEnumerable<T> entities);
        
        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Saves changes made in this repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// Defines a repository for accessing and manipulating entities with an integer identifier.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepository<T> : IRepository<T, int>, IReadOnlyRepository<T> where T : class, IEntity<int>
    {
    }
}
