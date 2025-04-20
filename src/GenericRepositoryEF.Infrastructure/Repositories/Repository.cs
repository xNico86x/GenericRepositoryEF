using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System.Data.Common;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the <see cref="IRepository{T, TKey}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    public class Repository<T, TKey, TContext> : ReadOnlyRepository<T, TKey, TContext>, IRepository<T, TKey>
        where T : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T, TKey, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public Repository(TContext dbContext, ILogger<Repository<T, TKey, TContext>> logger)
            : base(dbContext, logger)
        {
        }

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Adding entity of type {EntityType}", typeof(T).Name);
            
            await _dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Adding range of entities of type {EntityType}", typeof(T).Name);
            
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            _logger.LogDebug("Updating entity of type {EntityType} with id {EntityId}", typeof(T).Name, entity.Id);
            
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _logger.LogDebug("Updating range of entities of type {EntityType}", typeof(T).Name);
            
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            _logger.LogDebug("Deleting entity of type {EntityType} with id {EntityId}", typeof(T).Name, entity.Id);
            
            if (entity is ISoftDelete softDelete)
            {
                softDelete.IsDeleted = true;
                softDelete.DeletedDate = DateTime.UtcNow;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        /// <inheritdoc />
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _logger.LogDebug("Deleting range of entities of type {EntityType}", typeof(T).Name);
            
            var softDeleteEntities = new List<T>();
            var hardDeleteEntities = new List<T>();
            
            foreach (var entity in entities)
            {
                if (entity is ISoftDelete)
                {
                    softDeleteEntities.Add(entity);
                }
                else
                {
                    hardDeleteEntities.Add(entity);
                }
            }
            
            foreach (var entity in softDeleteEntities)
            {
                if (entity is ISoftDelete softDelete)
                {
                    softDelete.IsDeleted = true;
                    softDelete.DeletedDate = DateTime.UtcNow;
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }
            }
            
            if (hardDeleteEntities.Any())
            {
                _dbSet.RemoveRange(hardDeleteEntities);
            }
        }

        /// <inheritdoc />
        public virtual async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Deleting entity of type {EntityType} with id {EntityId}", typeof(T).Name, id);
            
            var entity = await GetByIdAsync(id, cancellationToken);
            
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, id);
            }
            
            Delete(entity);
        }

        /// <inheritdoc />
        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Saving changes for repository of type {EntityType}", typeof(T).Name);
            
            try
            {
                // Using Polly for retry logic on transient database errors
                var retryPolicy = Policy
                    .Handle<DbException>()
                    .Or<DbUpdateException>(ex => ex.InnerException is DbException)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            _logger.LogWarning(exception, 
                                "Error saving changes for repository of type {EntityType}. Retry attempt {RetryCount}", 
                                typeof(T).Name, retryCount);
                        });

                return await retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        // Handle audit properties (CreatedDate, LastModifiedDate)
                        UpdateAuditProperties();
                        
                        return await _dbContext.SaveChangesAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        _logger.LogError(ex, "Concurrency error saving changes for repository of type {EntityType}", typeof(T).Name);
                        throw new ConcurrencyException($"A concurrency error occurred while saving changes for {typeof(T).Name}", typeof(T).Name);
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError(ex, "Error saving changes for repository of type {EntityType}", typeof(T).Name);
                        throw new RepositoryException($"An error occurred while saving changes for {typeof(T).Name}", ex);
                    }
                });
            }
            catch (Exception ex) when (!(ex is ConcurrencyException || ex is RepositoryException))
            {
                _logger.LogError(ex, "Unexpected error saving changes for repository of type {EntityType}", typeof(T).Name);
                throw new RepositoryException($"An unexpected error occurred while saving changes for {typeof(T).Name}", ex);
            }
        }
        
        /// <summary>
        /// Updates audit properties for entities implementing IAuditableEntity.
        /// </summary>
        private void UpdateAuditProperties()
        {
            var entries = _dbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditableEntity && (
                    e.State == EntityState.Added || 
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is IAuditableEntity auditableEntity)
                {
                    var now = DateTime.UtcNow;
                    var userId = GetCurrentUserId();
                    
                    if (entityEntry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedDate = now;
                        auditableEntity.CreatedBy = userId;
                    }
                    else
                    {
                        auditableEntity.LastModifiedDate = now;
                        auditableEntity.LastModifiedBy = userId;
                        
                        // Don't modify CreatedDate and CreatedBy
                        entityEntry.Property("CreatedDate").IsModified = false;
                        entityEntry.Property("CreatedBy").IsModified = false;
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets the current user identifier.
        /// </summary>
        /// <returns>The current user identifier.</returns>
        protected virtual string GetCurrentUserId()
        {
            // In a real application, this would likely come from an IHttpContextAccessor, ClaimsPrincipal, etc.
            // For simplicity, we're returning a placeholder value.
            return "system";
        }
    }

    /// <summary>
    /// Implementation of the <see cref="IRepository{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    public class Repository<T, TContext> : Repository<T, int, TContext>, IRepository<T>
        where T : class, IEntity<int>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public Repository(TContext dbContext, ILogger<Repository<T, int, TContext>> logger)
            : base(dbContext, logger)
        {
        }
    }
}
