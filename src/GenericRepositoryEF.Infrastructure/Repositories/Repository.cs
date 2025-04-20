using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of a repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await DbSet.AddAsync(entity, cancellationToken);
                return entity;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "AddAsync", ex);
            }
        }

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                await DbSet.AddRangeAsync(entities, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "AddRangeAsync", ex);
            }
        }

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated entity.</returns>
        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                DbSet.Update(entity);
                return Task.FromResult(entity);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (entity is IEntityWithKey<object> entityWithKey)
                {
                    throw new ConcurrencyException(typeof(T), entityWithKey.Id, ex);
                }

                throw new RepositoryException(typeof(T), "UpdateAsync", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "UpdateAsync", ex);
            }
        }

        /// <summary>
        /// Updates a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                DbSet.UpdateRange(entities);
                return Task.CompletedTask;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("A concurrency conflict occurred while updating a range of entities.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "UpdateRangeAsync", ex);
            }
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                DbSet.Remove(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "DeleteAsync", ex);
            }
        }

        /// <summary>
        /// Deletes a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            try
            {
                DbSet.RemoveRange(entities);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "DeleteRangeAsync", ex);
            }
        }

        /// <summary>
        /// Deletes entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities deleted.</returns>
        public async Task<int> DeleteAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                var entities = await ListAsync(specification, cancellationToken);
                
                if (entities.Count == 0)
                {
                    return 0;
                }

                DbSet.RemoveRange(entities);
                return entities.Count;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "DeleteAsync with specification", ex);
            }
        }
    }

    /// <summary>
    /// Implementation of a repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class Repository<T, TKey> : Repository<T>, IRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(DbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Deletes an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the entity was deleted, false otherwise.</returns>
        public async Task<bool> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await GetByIdAsync(id, cancellationToken);
                
                if (entity == null)
                {
                    return false;
                }

                await DeleteAsync(entity, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), $"DeleteByIdAsync with id {id}", ex);
            }
        }
    }
}