using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Null object implementation of <see cref="IRepository{T, TKey}"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public class NullRepository<T, TKey> : IRepository<T, TKey>
        where T : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets an empty list of entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty list.</returns>
        public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <summary>
        /// Gets null entity by primary key.
        /// </summary>
        /// <param name="id">The entity's primary key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Null.</returns>
        public Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <summary>
        /// Gets an empty list of entities that match the predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty list.</returns>
        public Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <summary>
        /// Determines whether any entity matches the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>False.</returns>
        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Counts the number of entities that match the specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Zero.</returns>
        public Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets an empty paged result.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty paged result.</returns>
        public Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            var result = new PagedResult<T>
            {
                Items = new List<T>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = 0
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// Gets an empty list of entities that match the specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty list.</returns>
        public Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <summary>
        /// Gets null entity that matches the specification.
        /// </summary>
        /// <param name="specification">The specification to filter entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Null.</returns>
        public Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Adds multiple entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(T entity)
        {
            // No-op
        }

        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public void UpdateRange(IEnumerable<T> entities)
        {
            // No-op
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(T entity)
        {
            // No-op
        }

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        public void DeleteRange(IEnumerable<T> entities)
        {
            // No-op
        }

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Saves changes made in this repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Zero.</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Null object implementation of <see cref="IRepository{T}"/>.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class NullRepository<T> : NullRepository<T, int>, IRepository<T> 
        where T : class, IEntity<int>
    {
        // This class inherits all implementation from NullRepository<T, TKey>
    }
}