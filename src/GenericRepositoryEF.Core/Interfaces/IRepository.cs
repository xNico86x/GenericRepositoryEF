using GenericRepositoryEF.Core.Models;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        T Add(T entity);

        /// <summary>
        /// Adds a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity);

        /// <summary>
        /// Updates a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        void Delete(object id);

        /// <summary>
        /// Deletes a collection of entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes entities based on a specification.
        /// </summary>
        /// <param name="specification">The specification to match entities to delete.</param>
        void Delete(ISpecification<T> specification);

        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <returns>The number of entities saved.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities saved.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}