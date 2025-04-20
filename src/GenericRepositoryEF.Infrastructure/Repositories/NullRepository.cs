using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of a null repository (using the Null Object pattern).
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class NullRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty collection.</returns>
        public Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <summary>
        /// Gets entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An empty collection.</returns>
        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Null.</returns>
        public Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously, throws if no entity was found.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<T> FirstAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Cannot get first entity from a null repository.");
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Null.</returns>
        public Task<T?> SingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously, throws if no entity was found.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<T> SingleAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Cannot get single entity from a null repository.");
        }

        /// <summary>
        /// Counts entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Zero.</returns>
        public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Checks if any entity matches the specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>False.</returns>
        public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entity);
        }

        /// <summary>
        /// Updates a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Zero.</returns>
        public Task<int> DeleteAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Implementation of a null repository with a key (using the Null Object pattern).
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class NullRepository<T, TKey> : NullRepository<T>, IRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Null.</returns>
        public Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <summary>
        /// Gets an entity by its identifier asynchronously, throws if the entity does not exist.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Cannot get entity by id from a null repository.");
        }

        /// <summary>
        /// Deletes an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>False.</returns>
        public Task<bool> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}