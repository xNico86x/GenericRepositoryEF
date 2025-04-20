using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Cached repository implementation for Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class CachedRepository<T> : ICachedRepository<T> where T : class, IEntity
    {
        private readonly IRepository<T> _repository;
        private readonly IMemoryCache? _memoryCache;
        private readonly IDistributedCache? _distributedCache;
        private readonly MemoryCacheEntryOptions? _memoryCacheOptions;
        private readonly DistributedCacheEntryOptions? _distributedCacheOptions;
        private const int DefaultCacheExpirationMinutes = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheExpirationMinutes">The cache expiration in minutes. Default is 30 minutes.</param>
        public CachedRepository(
            IRepository<T> repository,
            IMemoryCache? memoryCache = null,
            IDistributedCache? distributedCache = null,
            int cacheExpirationMinutes = DefaultCacheExpirationMinutes)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;

            // Configure memory cache options if memory cache is available
            if (_memoryCache != null)
            {
                _memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(cacheExpirationMinutes / 2)
                };
            }

            // Configure distributed cache options if distributed cache is available
            if (_distributedCache != null)
            {
                _distributedCacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheExpirationMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(cacheExpirationMinutes / 2)
                };
            }
        }

        #region IRepository implementation

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>The entities.</returns>
        public IReadOnlyList<T> GetAll()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllAsync(cancellationToken);
        }

        /// <summary>
        /// Gets entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The entities.</returns>
        public IReadOnlyList<T> GetAll(ISpecification<T> specification)
        {
            return _repository.GetAll(specification);
        }

        /// <summary>
        /// Gets entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return _repository.GetAllAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets a single entity using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The entity or null.</returns>
        public T? GetFirstOrDefault(ISpecification<T> specification)
        {
            return _repository.GetFirstOrDefault(specification);
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        public Task<T?> GetFirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return _repository.GetFirstOrDefaultAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets a single entity using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The entity or null.</returns>
        public T? GetSingleOrDefault(ISpecification<T> specification)
        {
            return _repository.GetSingleOrDefault(specification);
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        public Task<T?> GetSingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return _repository.GetSingleOrDefaultAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets a paged result of entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged result.</returns>
        public PagedResult<T> GetPaged(ISpecification<T> specification, int pageNumber, int pageSize)
        {
            return _repository.GetPaged(specification, pageNumber, pageSize);
        }

        /// <summary>
        /// Gets a paged result of entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The paged result.</returns>
        public Task<PagedResult<T>> GetPagedAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return _repository.GetPagedAsync(specification, pageNumber, pageSize, cancellationToken);
        }

        /// <summary>
        /// Counts entities.
        /// </summary>
        /// <returns>The count.</returns>
        public int Count()
        {
            return _repository.Count();
        }

        /// <summary>
        /// Counts entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count.</returns>
        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return _repository.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Counts entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The count.</returns>
        public int Count(ISpecification<T> specification)
        {
            return _repository.Count(specification);
        }

        /// <summary>
        /// Counts entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count.</returns>
        public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return _repository.CountAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Checks if any entity matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if any entity matches the specification, otherwise false.</returns>
        public bool Any(ISpecification<T> specification)
        {
            return _repository.Any(specification);
        }

        /// <summary>
        /// Checks if any entity matches the specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity matches the specification, otherwise false.</returns>
        public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return _repository.AnyAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The entity or null.</returns>
        public T? Find(object id)
        {
            var cacheKey = GetCacheKey(id);
            var cachedEntity = GetFromCache(cacheKey);
            
            if (cachedEntity != null)
            {
                return cachedEntity;
            }

            var entity = _repository.Find(id);
            
            if (entity != null)
            {
                SetCache(cacheKey, entity);
            }

            return entity;
        }

        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        public async Task<T?> FindAsync(object id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(id);
            var cachedEntity = await GetFromCacheAsync(cacheKey, cancellationToken);
            
            if (cachedEntity != null)
            {
                return cachedEntity;
            }

            var entity = await _repository.FindAsync(id, cancellationToken);
            
            if (entity != null)
            {
                await SetCacheAsync(cacheKey, entity, cancellationToken);
            }

            return entity;
        }

        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        public T Add(T entity)
        {
            var result = _repository.Add(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            SetCache(cacheKey, result);
            
            return result;
        }

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _repository.AddAsync(entity, cancellationToken);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, result, cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Adds a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void AddRange(IEnumerable<T> entities)
        {
            _repository.AddRange(entities);
        }

        /// <summary>
        /// Adds a range of entities asynchronously.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return _repository.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity.</returns>
        public T Update(T entity)
        {
            var result = _repository.Update(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            SetCache(cacheKey, result);
            
            return result;
        }

        /// <summary>
        /// Updates a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void UpdateRange(IEnumerable<T> entities)
        {
            _repository.UpdateRange(entities);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                SetCache(cacheKey, entity);
            }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            var cacheKey = GetCacheKey(entity.Id);
            RemoveFromCache(cacheKey);
            
            _repository.Delete(entity);
        }

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Delete(object id)
        {
            var cacheKey = GetCacheKey(id);
            RemoveFromCache(cacheKey);
            
            _repository.Delete(id);
        }

        /// <summary>
        /// Deletes a range of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                RemoveFromCache(cacheKey);
            }
            
            _repository.DeleteRange(entities);
        }

        #endregion

        #region ICachedRepository implementation

        /// <summary>
        /// Refreshes the cache for an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>True if the cache was refreshed, otherwise false.</returns>
        public bool RefreshCache(object id)
        {
            var entity = _repository.Find(id);
            
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
            var entity = await _repository.FindAsync(id, cancellationToken);
            
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
            var entities = _repository.GetAll(specification);
            
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
            var entities = await _repository.GetAllAsync(specification, cancellationToken);
            
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
            var entities = _repository.GetAll();
            
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
            var entities = await _repository.GetAllAsync(cancellationToken);
            
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
            
            if (_memoryCache != null)
            {
                _memoryCache.Remove(cacheKey);
            }
            
            if (_distributedCache != null)
            {
                _distributedCache.Remove(cacheKey);
            }
            
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
            
            if (_memoryCache != null)
            {
                _memoryCache.Remove(cacheKey);
            }
            
            if (_distributedCache != null)
            {
                await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
            }
            
            return true;
        }

        /// <summary>
        /// Clears the entire cache for the entity type.
        /// </summary>
        /// <returns>True if the cache was cleared, otherwise false.</returns>
        public bool ClearCache()
        {
            var collectionCacheKey = GetCollectionCacheKey();
            
            if (_memoryCache != null)
            {
                _memoryCache.Remove(collectionCacheKey);
            }
            
            if (_distributedCache != null)
            {
                _distributedCache.Remove(collectionCacheKey);
            }

            var entities = _repository.GetAll();
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                
                if (_memoryCache != null)
                {
                    _memoryCache.Remove(cacheKey);
                }
                
                if (_distributedCache != null)
                {
                    _distributedCache.Remove(cacheKey);
                }
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
            
            if (_memoryCache != null)
            {
                _memoryCache.Remove(collectionCacheKey);
            }
            
            if (_distributedCache != null)
            {
                await _distributedCache.RemoveAsync(collectionCacheKey, cancellationToken);
            }

            var entities = await _repository.GetAllAsync(cancellationToken);
            
            foreach (var entity in entities)
            {
                var cacheKey = GetCacheKey(entity.Id);
                
                if (_memoryCache != null)
                {
                    _memoryCache.Remove(cacheKey);
                }
                
                if (_distributedCache != null)
                {
                    await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
                }
            }
            
            return true;
        }

        #endregion

        #region Cache helpers

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
            // Try memory cache first
            if (_memoryCache != null && _memoryCache.TryGetValue(cacheKey, out T? cachedItem))
            {
                return cachedItem;
            }
            
            // Then try distributed cache
            if (_distributedCache != null)
            {
                var cachedData = _distributedCache.Get(cacheKey);
                
                if (cachedData != null)
                {
                    var entity = JsonSerializer.Deserialize<T>(cachedData);
                    
                    // Store in memory cache for faster access next time
                    if (_memoryCache != null && entity != null)
                    {
                        _memoryCache.Set(cacheKey, entity, _memoryCacheOptions);
                    }
                    
                    return entity;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Gets an entity from the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity or null.</returns>
        protected virtual async Task<T?> GetFromCacheAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            // Try memory cache first
            if (_memoryCache != null && _memoryCache.TryGetValue(cacheKey, out T? cachedItem))
            {
                return cachedItem;
            }
            
            // Then try distributed cache
            if (_distributedCache != null)
            {
                var cachedData = await _distributedCache.GetAsync(cacheKey, cancellationToken);
                
                if (cachedData != null)
                {
                    var entity = JsonSerializer.Deserialize<T>(cachedData);
                    
                    // Store in memory cache for faster access next time
                    if (_memoryCache != null && entity != null)
                    {
                        _memoryCache.Set(cacheKey, entity, _memoryCacheOptions);
                    }
                    
                    return entity;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Sets an entity in the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="entity">The entity.</param>
        protected virtual void SetCache(string cacheKey, T entity)
        {
            // Set in memory cache
            if (_memoryCache != null)
            {
                _memoryCache.Set(cacheKey, entity, _memoryCacheOptions);
            }
            
            // Set in distributed cache
            if (_distributedCache != null)
            {
                var cacheData = JsonSerializer.SerializeToUtf8Bytes(entity);
                _distributedCache.Set(cacheKey, cacheData, _distributedCacheOptions);
            }
        }

        /// <summary>
        /// Sets an entity in the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        protected virtual async Task SetCacheAsync(string cacheKey, T entity, CancellationToken cancellationToken = default)
        {
            // Set in memory cache
            if (_memoryCache != null)
            {
                _memoryCache.Set(cacheKey, entity, _memoryCacheOptions);
            }
            
            // Set in distributed cache
            if (_distributedCache != null)
            {
                var cacheData = JsonSerializer.SerializeToUtf8Bytes(entity);
                await _distributedCache.SetAsync(cacheKey, cacheData, _distributedCacheOptions, cancellationToken);
            }
        }

        #endregion

        #region Missing methods required by IRepository implementation

        /// <summary>
        /// Deletes entities based on a specification.
        /// </summary>
        /// <param name="specification">The specification to match entities to delete.</param>
        public void Delete(ISpecification<T> specification)
        {
            var entities = GetAll(specification);
            DeleteRange(entities);
        }
        
        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <returns>The number of entities saved.</returns>
        public int SaveChanges()
        {
            return _repository.SaveChanges();
        }
        
        /// <summary>
        /// Saves changes to the repository.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities saved.</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _repository.SaveChangesAsync(cancellationToken);
        }
        
        #endregion
    }
}