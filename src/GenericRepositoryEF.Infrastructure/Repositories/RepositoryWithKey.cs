using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Entity Framework Core with a typed key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class Repository<T, TKey> : Repository<T>, IRepository<T, TKey> 
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T, TKey}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        public Repository(DbContext context, ISpecificationEvaluator specificationEvaluator)
            : base(context, specificationEvaluator)
        {
        }

        /// <summary>
        /// Gets an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity or null.</returns>
        public virtual T? GetById(TKey id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Gets an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Gets an entity by ID or throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity.</returns>
        public virtual T GetByIdOrThrow(TKey id)
        {
            var entity = GetById(id);
            
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }
            
            return entity;
        }

        /// <summary>
        /// Gets an entity by ID asynchronously or throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public virtual async Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }
            
            return entity;
        }

        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public virtual void Delete(TKey id)
        {
            Delete((object)id);
        }

        /// <summary>
        /// Deletes an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            
            if (entity != null)
            {
                Delete(entity);
            }
        }
    }
}