using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Decorator for repositories adding caching capabilities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public class CachedRepository<T, TKey> : ICachedRepository<T, TKey>
        where T : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRepository<T, TKey> _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedRepository<T, TKey>> _logger;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        
        // Cache keys
        private readonly string _allEntitiesKey;
        private readonly string _entityTypePrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The underlying repository.</param>
        /// <param name="cache">The memory cache.</param>
        /// <param name="logger">The logger.</param>
        public CachedRepository(
            IRepository<T, TKey> repository,
            IMemoryCache cache,
            ILogger<CachedRepository<T, TKey>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // Default cache options - 10 minutes sliding expiration
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                
            _entityTypePrefix = $"Entity_{typeof(T).Name}_";
            _allEntitiesKey = $"{_entityTypePrefix}All";
        }

        /// <inheritdoc />
        public void InvalidateCache()
        {
            _logger.LogDebug("Invalidating cache for entity type {EntityType}", typeof(T).Name);
            
            _cache.Remove(_allEntitiesKey);
        }

        /// <inheritdoc />
        public void InvalidateCacheItem(TKey id)
        {
            _logger.LogDebug("Invalidating cache for entity type {EntityType} with id {EntityId}", typeof(T).Name, id);
            
            _cache.Remove(GetEntityKey(id));
        }

        /// <inheritdoc />
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _repository.AddAsync(entity, cancellationToken);
            InvalidateCache();
            return result;
        }

        /// <inheritdoc />
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _repository.AddRangeAsync(entities, cancellationToken);
            InvalidateCache();
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            // Don't cache counts as they may change frequently
            return await _repository.CountAsync(specification, cancellationToken);
        }

        /// <inheritdoc />
        public void Delete(T entity)
        {
            _repository.Delete(entity);
            
            if (entity.Id != null)
            {
                InvalidateCacheItem(entity.Id);
            }
            
            InvalidateCache();
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteByIdAsync(id, cancellationToken);
            InvalidateCacheItem(id);
            InvalidateCache();
        }

        /// <inheritdoc />
        public void DeleteRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
            
            foreach (var entity in entities)
            {
                if (entity.Id != null)
                {
                    InvalidateCacheItem(entity.Id);
                }
            }
            
            InvalidateCache();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            // Don't cache exists queries as they may change frequently
            return await _repository.ExistsAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cache.GetOrCreateAsync(_allEntitiesKey, async entry => 
            {
                entry.SetOptions(_cacheOptions);
                _logger.LogDebug("Cache miss for all entities of type {EntityType}", typeof(T).Name);
                return await _repository.GetAllAsync(cancellationToken);
            });
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            // Don't cache predicate-based queries as they are too varied
            return await _repository.GetAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetEntityKey(id);
            
            return await _cache.GetOrCreateAsync(cacheKey, async entry => 
            {
                entry.SetOptions(_cacheOptions);
                _logger.LogDebug("Cache miss for entity of type {EntityType} with id {EntityId}", typeof(T).Name, id);
                return await _repository.GetByIdAsync(id, cancellationToken);
            });
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // Don't cache specification-based queries as they are too varied
            return await _repository.GetBySpecificationAsync(specification, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            // Don't cache paged results as they are too varied
            return await _repository.GetPagedAsync(pageNumber, pageSize, specification, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // Don't cache specification-based queries as they are too varied
            return await _repository.GetSingleBySpecificationAsync(specification, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repository.SaveChangesAsync(cancellationToken);
            InvalidateCache();
            return result;
        }

        /// <inheritdoc />
        public void Update(T entity)
        {
            _repository.Update(entity);
            
            if (entity.Id != null)
            {
                InvalidateCacheItem(entity.Id);
            }
            
            InvalidateCache();
        }

        /// <inheritdoc />
        public void UpdateRange(IEnumerable<T> entities)
        {
            _repository.UpdateRange(entities);
            
            foreach (var entity in entities)
            {
                if (entity.Id != null)
                {
                    InvalidateCacheItem(entity.Id);
                }
            }
            
            InvalidateCache();
        }
        
        /// <summary>
        /// Gets the cache key for an entity with the specified identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>The cache key.</returns>
        private string GetEntityKey(TKey id) => $"{_entityTypePrefix}{id}";
    }
    
    /// <summary>
    /// Decorator for repositories adding caching capabilities for entities with an integer identifier.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class CachedRepository<T> : CachedRepository<T, int>, ICachedRepository<T>
        where T : class, IEntity<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T}"/> class.
        /// </summary>
        /// <param name="repository">The underlying repository.</param>
        /// <param name="cache">The memory cache.</param>
        /// <param name="logger">The logger.</param>
        public CachedRepository(
            IRepository<T> repository,
            IMemoryCache cache,
            ILogger<CachedRepository<T, int>> logger)
            : base(repository, cache, logger)
        {
        }
    }
}
