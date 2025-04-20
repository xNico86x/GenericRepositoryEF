using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Cached repository implementation for Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class CachedRepository<T> : Repository<T>, ICachedRepository<T> where T : class, IEntity
    {
        private readonly IDistributedCache _cache;
        private readonly IDateTime _dateTime;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="dateTime">The date time service.</param>
        /// <param name="cacheExpirationMinutes">The cache expiration in minutes. Default is 30 minutes.</param>
        public CachedRepository(
            DbContext context,
            ISpecificationEvaluator specificationEvaluator,
            IDistributedCache cache,
            IDateTime dateTime,
            int cacheExpirationMinutes = 30)
            : base(context, specificationEvaluator)
        {
            _cache = cache;
            _dateTime = dateTime;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheExpirationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(cacheExpirationMinutes / 2)
            };
        }

        /// <summary>
        /// Finds an entity by ID with caching.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <returns>The found entity or null.</returns>
        public override T? Find(object id)
        {
            var cacheKey = GetCacheKey(id);
            var cachedEntity = GetFromCache(cacheKey);
            
            if (cachedEntity != null)
            {
                return cachedEntity;
            }

            var entity = base.Find(id);
            
            if (entity != null)
            {
                SetCache(cacheKey, entity);
            }

            return entity;
        }

        /// <summary>
        /// Finds an entity by ID with caching.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        public override async Task<T?> FindAsync(object id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(id);
            var cachedEntity = await GetFromCacheAsync(cacheKey, cancellationToken);
            
            if (cachedEntity != null)
            {
                return cachedEntity;
            }

            var entity = await base.FindAsync(id, cancellationToken);
            
            if (entity != null)
            {
                await SetCacheAsync(cacheKey, entity, cancellationToken);
            }

            return entity;
        }

        /// <summary>
        /// Adds a new entity and invalidates cache.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public override T Add(T entity)
        {
            var result = base.Add(entity);
            SaveChanges();
            
            var cacheKey = GetCacheKey(entity.Id);
            SetCache(cacheKey, result);
            
            return result;
        }

        /// <summary>
        /// Adds a new entity and invalidates cache.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await base.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, result, cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Updates an entity and invalidates cache.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public override T Update(T entity)
        {
            var result = base.Update(entity);
            SaveChanges();
            
            var cacheKey = GetCacheKey(entity.Id);
            SetCache(cacheKey, result);
            
            return result;
        }

        /// <summary>
        /// Deletes an entity and invalidates cache.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public override void Delete(T entity)
        {
            var cacheKey = GetCacheKey(entity.Id);
            RemoveFromCache(cacheKey);
            
            base.Delete(entity);
            SaveChanges();
        }

        /// <summary>
        /// Deletes an entity by ID and invalidates cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        public override void Delete(object id)
        {
            var cacheKey = GetCacheKey(id);
            RemoveFromCache(cacheKey);
            
            base.Delete(id);
            SaveChanges();
        }

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public bool RefreshCache(object id)
        {
            var entity = base.Find(id);
            
            if (entity == null)
            {
                return false;
            }
            
            var cacheKey = GetCacheKey(id);
            SetCache(cacheKey, entity);
            
            return true;
        }

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public async Task<bool> RefreshCacheAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await base.FindAsync(id, cancellationToken);
            
            if (entity == null)
            {
                return false;
            }
            
            var cacheKey = GetCacheKey(id);
            await SetCacheAsync(cacheKey, entity, cancellationToken);
            
            return true;
        }

        /// <summary>
        /// Refreshes the cache for a collection of entities.
        /// </summary>
        /// <param name="specification">The specification to refresh.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public bool RefreshCache(ISpecification<T> specification)
        {
            var entities = base.GetAll(specification);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                SetCache(cacheKey, entity);
            }
            
            return true;
        }

        /// <summary>
        /// Refreshes the cache for a collection of entities.
        /// </summary>
        /// <param name="specification">The specification to refresh.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public async Task<bool> RefreshCacheAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var entities = await base.GetAllAsync(specification, cancellationToken);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                await SetCacheAsync(cacheKey, entity, cancellationToken);
            }
            
            return true;
        }

        /// <summary>
        /// Refreshes the entire cache for the entity type.
        /// </summary>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public bool RefreshEntireCache()
        {
            var collectionCacheKey = GetCollectionCacheKey();
            var entities = base.GetAll();
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                SetCache(cacheKey, entity);
            }
            
            return true;
        }

        /// <summary>
        /// Refreshes the entire cache for the entity type.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public async Task<bool> RefreshEntireCacheAsync(CancellationToken cancellationToken = default)
        {
            var collectionCacheKey = GetCollectionCacheKey();
            var entities = await base.GetAllAsync(cancellationToken);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                await SetCacheAsync(cacheKey, entity, cancellationToken);
            }
            
            return true;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        public bool RemoveFromCache(object id)
        {
            var cacheKey = GetCacheKey(id);
            _cache.Remove(cacheKey);
            
            return true;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the item was removed, otherwise false.</returns>
        public async Task<bool> RemoveFromCacheAsync(object id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(id);
            await _cache.RemoveAsync(cacheKey, cancellationToken);
            
            return true;
        }

        /// <summary>
        /// Clears the entire cache for the entity type.
        /// </summary>
        /// <returns>True if the cache was cleared, otherwise false.</returns>
        public bool ClearCache()
        {
            var collectionCacheKey = GetCollectionCacheKey();
            _cache.Remove(collectionCacheKey);

            var entities = base.GetAll();
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                _cache.Remove(cacheKey);
            }
            
            return true;
        }

        /// <summary>
        /// Clears the entire cache for the entity type.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the cache was cleared, otherwise false.</returns>
        public async Task<bool> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            var collectionCacheKey = GetCollectionCacheKey();
            await _cache.RemoveAsync(collectionCacheKey, cancellationToken);

            var entities = await base.GetAllAsync(cancellationToken);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                await _cache.RemoveAsync(cacheKey, cancellationToken);
            }
            
            return true;
        }

        /// <summary>
        /// Gets a cache key for an entity.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The cache key.</returns>
        protected virtual string GetCacheKey(object id)
        {
            return $"{typeof(T).Name}_{id}";
        }

        /// <summary>
        /// Gets a cache key for a collection of entities.
        /// </summary>
        /// <returns>The cache key.</returns>
        protected virtual string GetCollectionCacheKey()
        {
            return $"{typeof(T).Name}_All";
        }

        /// <summary>
        /// Gets an entity from the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The entity or null.</returns>
        protected virtual T? GetFromCache(string cacheKey)
        {
            var cachedData = _cache.Get(cacheKey);
            
            if (cachedData == null)
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<T>(cachedData);
        }

        /// <summary>
        /// Gets an entity from the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        protected virtual async Task<T?> GetFromCacheAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            var cachedData = await _cache.GetAsync(cacheKey, cancellationToken);
            
            if (cachedData == null)
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<T>(cachedData);
        }

        /// <summary>
        /// Sets an entity in the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="entity">The entity.</param>
        protected virtual void SetCache(string cacheKey, T entity)
        {
            var cacheData = JsonSerializer.SerializeToUtf8Bytes(entity);
            _cache.Set(cacheKey, cacheData, _cacheOptions);
        }

        /// <summary>
        /// Sets an entity in the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        protected virtual async Task SetCacheAsync(string cacheKey, T entity, CancellationToken cancellationToken = default)
        {
            var cacheData = JsonSerializer.SerializeToUtf8Bytes(entity);
            await _cache.SetAsync(cacheKey, cacheData, _cacheOptions, cancellationToken);
        }
    }
}