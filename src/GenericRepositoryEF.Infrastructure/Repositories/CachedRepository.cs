using System.Text.Json;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Repository with caching support.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class CachedRepository<T> : Repository<T>, ICachedRepository<T> where T : class, IEntity
    {
        private readonly IDistributedCache _cache;
        private readonly string _cacheKeyPrefix;
        private string _customCacheKey;
        private int _cacheDurationMinutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        /// <param name="cache">The distributed cache.</param>
        public CachedRepository(DbContext dbContext, ISpecificationEvaluator specificationEvaluator, IDistributedCache cache)
            : base(dbContext, specificationEvaluator)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cacheKeyPrefix = $"Repository_{typeof(T).Name}_";
            _cacheDurationMinutes = 30; // Default cache duration
        }

        /// <inheritdoc/>
        public ICachedRepository<T> WithCacheDuration(int minutes)
        {
            _cacheDurationMinutes = minutes > 0 ? minutes : 30;
            return this;
        }

        /// <inheritdoc/>
        public ICachedRepository<T> WithCacheKey(string key)
        {
            _customCacheKey = !string.IsNullOrWhiteSpace(key) ? key : $"{_cacheKeyPrefix}Default";
            return this;
        }

        /// <inheritdoc/>
        public async Task InvalidateCacheAsync()
        {
            var cacheKey = GetCacheKey();
            await _cache.RemoveAsync(cacheKey);
        }

        /// <inheritdoc/>
        public async Task RefreshCacheAsync()
        {
            await InvalidateCacheAsync();
            // The next query will repopulate the cache
        }

        /// <inheritdoc/>
        public override async Task<List<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey();
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<T>>(cachedData);
            }

            var data = await base.ListAllAsync(cancellationToken);
            await CacheDataAsync(cacheKey, data, cancellationToken);
            return data;
        }

        /// <inheritdoc/>
        public override async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(specification);
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<T>>(cachedData);
            }

            var data = await base.ListAsync(specification, cancellationToken);
            await CacheDataAsync(cacheKey, data, cancellationToken);
            return data;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The cache key.</returns>
        protected virtual string GetCacheKey(ISpecification<T> specification = null)
        {
            if (!string.IsNullOrEmpty(_customCacheKey))
            {
                return _customCacheKey;
            }

            if (specification != null)
            {
                return $"{_cacheKeyPrefix}{specification.GetType().Name}";
            }

            return $"{_cacheKeyPrefix}All";
        }

        /// <summary>
        /// Caches data.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="data">The data to cache.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task CacheDataAsync(string cacheKey, object data, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheDurationMinutes)
            };

            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(cacheKey, serializedData, options, cancellationToken);
        }
    }

    /// <summary>
    /// Repository with caching support and a specific key type.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public class CachedRepository<T, TKey> : Repository<T, TKey>, ICachedRepository<T, TKey> where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        private readonly IDistributedCache _cache;
        private readonly string _cacheKeyPrefix;
        private string _customCacheKey;
        private int _cacheDurationMinutes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        /// <param name="cache">The distributed cache.</param>
        public CachedRepository(DbContext dbContext, ISpecificationEvaluator specificationEvaluator, IDistributedCache cache)
            : base(dbContext, specificationEvaluator)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cacheKeyPrefix = $"Repository_{typeof(T).Name}_";
            _cacheDurationMinutes = 30; // Default cache duration
        }

        /// <inheritdoc/>
        public ICachedRepository<T, TKey> WithCacheDuration(int minutes)
        {
            _cacheDurationMinutes = minutes > 0 ? minutes : 30;
            return this;
        }

        /// <inheritdoc/>
        public ICachedRepository<T, TKey> WithCacheKey(string key)
        {
            _customCacheKey = !string.IsNullOrWhiteSpace(key) ? key : $"{_cacheKeyPrefix}Default";
            return this;
        }

        /// <inheritdoc/>
        public async Task InvalidateCacheAsync()
        {
            var cacheKey = GetCacheKey();
            await _cache.RemoveAsync(cacheKey);
        }

        /// <inheritdoc/>
        public async Task RefreshCacheAsync()
        {
            await InvalidateCacheAsync();
            // The next query will repopulate the cache
        }

        /// <inheritdoc/>
        public override async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey($"Id_{id}");
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<T>(cachedData);
            }

            var data = await base.GetByIdAsync(id, cancellationToken);
            if (data != null)
            {
                await CacheDataAsync(cacheKey, data, cancellationToken);
            }
            return data;
        }

        /// <inheritdoc/>
        public override async Task<List<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey();
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<T>>(cachedData);
            }

            var data = await base.ListAllAsync(cancellationToken);
            await CacheDataAsync(cacheKey, data, cancellationToken);
            return data;
        }

        /// <inheritdoc/>
        public override async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(specification);
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<List<T>>(cachedData);
            }

            var data = await base.ListAsync(specification, cancellationToken);
            await CacheDataAsync(cacheKey, data, cancellationToken);
            return data;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="specification">The specification or any other string to make the key unique.</param>
        /// <returns>The cache key.</returns>
        protected virtual string GetCacheKey(object specification = null)
        {
            if (!string.IsNullOrEmpty(_customCacheKey))
            {
                return _customCacheKey;
            }

            if (specification != null)
            {
                var specName = specification is ISpecification<T> ? specification.GetType().Name : specification.ToString();
                return $"{_cacheKeyPrefix}{specName}";
            }

            return $"{_cacheKeyPrefix}All";
        }

        /// <summary>
        /// Caches data.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="data">The data to cache.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task CacheDataAsync(string cacheKey, object data, CancellationToken cancellationToken)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheDurationMinutes)
            };

            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(cacheKey, serializedData, options, cancellationToken);
        }
    }
}