using GenericRepositoryEF.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Cached repository implementation for Entity Framework Core with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class CachedRepository<T, TKey> : CachedRepository<T>, ICachedRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        private readonly IRepository<T, TKey> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public CachedRepository(
            IRepository<T, TKey> repository,
            IMemoryCache? memoryCache = null,
            IDistributedCache? distributedCache = null)
            : base(repository, memoryCache, distributedCache)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets an entity by ID with caching.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity or null.</returns>
        public T? GetById(TKey id)
        {
            return Find(id);
        }

        /// <summary>
        /// Gets an entity by ID with caching.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(id, cancellationToken);
        }

        /// <summary>
        /// Gets an entity by ID with caching, throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity.</returns>
        public T GetByIdOrThrow(TKey id)
        {
            var entity = GetById(id);
            
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }
            
            return entity;
        }

        /// <summary>
        /// Gets an entity by ID with caching, throws if not found.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public async Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }
            
            return entity;
        }

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public bool RefreshCache(TKey id)
        {
            return RefreshCache((object)id);
        }

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public Task<bool> RefreshCacheAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return RefreshCacheAsync((object)id, cancellationToken);
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        public bool RemoveFromCache(TKey id)
        {
            return RemoveFromCache((object)id);
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        public Task<bool> RemoveFromCacheAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return RemoveFromCacheAsync((object)id, cancellationToken);
        }
        
        /// <summary>
        /// Deletes an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public void Delete(TKey id)
        {
            var cacheKey = GetCacheKey(id);
            RemoveFromCache(cacheKey);
            
            _repository.Delete(id);
        }
        
        /// <summary>
        /// Deletes an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(id);
            await RemoveFromCacheAsync(cacheKey, cancellationToken);
            
            await _repository.DeleteAsync(id, cancellationToken);
        }
    }
}