using System.Text.Json;
using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of a cached repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class CachedRepository<T> : ICachedRepository<T> where T : class, IEntity
    {
        private readonly IRepository<T> _repository;
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);
        
        private readonly string _cacheKeyPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cache">The cache.</param>
        public CachedRepository(IRepository<T> repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
            _cacheKeyPrefix = $"GenericRepositoryEF:{typeof(T).Name}";
            _cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            };
        }

        /// <summary>
        /// Creates a cache key.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The cache key.</returns>
        protected virtual string CreateCacheKey(string action, params object?[] parameters)
        {
            var parameterString = parameters != null && parameters.Length > 0
                ? ":" + string.Join(":", parameters.Select(p => p?.ToString() ?? "null"))
                : string.Empty;

            return $"{_cacheKeyPrefix}:{action}{parameterString}";
        }

        /// <summary>
        /// Gets a value from the cache.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value from the cache.</returns>
        protected virtual async Task<TValue?> GetFromCacheAsync<TValue>(string cacheKey, CancellationToken cancellationToken = default)
        {
            var cachedBytes = await _cache.GetAsync(cacheKey, cancellationToken);
            if (cachedBytes == null || cachedBytes.Length == 0)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TValue>(cachedBytes);
        }

        /// <summary>
        /// Sets a value in the cache.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task SetInCacheAsync<TValue>(string cacheKey, TValue value, CancellationToken cancellationToken = default)
        {
            var serializedValue = JsonSerializer.SerializeToUtf8Bytes(value);
            await _cache.SetAsync(cacheKey, serializedValue, _cacheOptions, cancellationToken);
        }

        /// <summary>
        /// Invalidates the cache.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task InvalidateCacheAsync(CancellationToken cancellationToken = default)
        {
            // This is a simplified approach; in a real-world scenario,
            // you might use a cache tag or prefix to group and invalidate related entries
            // Currently, we're clearing all cache entries for this entity type
            var cacheKey = CreateCacheKey("*");
            await _cache.RemoveAsync(cacheKey, cancellationToken);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All entities.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey("ListAll");
            var cachedResult = await GetFromCacheAsync<List<T>>(cacheKey, cancellationToken);
            
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = await _repository.ListAllAsync(cancellationToken);
            await SetInCacheAsync(cacheKey, result, cancellationToken);
            
            return result;
        }

        /// <summary>
        /// Gets entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities that match the specification.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // For specification-based queries, we currently don't cache
            // In a real-world scenario, you might implement caching based on the specification's unique properties
            return await _repository.ListAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets the first entity that matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // For specification-based queries, we currently don't cache
            return await _repository.FirstOrDefaultAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Gets the first entity that matches the specification or throws an exception.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="throwIfNotFound">If true, throws an exception if no entity matches the specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        public virtual async Task<T> FirstOrDefaultAsync(ISpecification<T> specification, bool throwIfNotFound, CancellationToken cancellationToken = default)
        {
            return await _repository.FirstOrDefaultAsync(specification, throwIfNotFound, cancellationToken);
        }

        /// <summary>
        /// Counts entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of entities that match the specification.</returns>
        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // For specification-based queries, we currently don't cache
            return await _repository.CountAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Checks if any entity matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity matches the specification.</returns>
        public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            // For specification-based queries, we currently don't cache
            return await _repository.AnyAsync(specification, cancellationToken);
        }

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added entity.</returns>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _repository.AddAsync(entity, cancellationToken);
            await InvalidateCacheAsync(cancellationToken);
            return result;
        }

        /// <summary>
        /// Adds a range of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _repository.AddRangeAsync(entities, cancellationToken);
            await InvalidateCacheAsync(cancellationToken);
        }

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity)
        {
            var result = _repository.Update(entity);
            InvalidateCacheAsync().GetAwaiter().GetResult();
            return result;
        }

        /// <summary>
        /// Updates a range of entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _repository.UpdateRange(entities);
            InvalidateCacheAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>The deleted entity.</returns>
        public virtual T Delete(T entity)
        {
            var result = _repository.Delete(entity);
            InvalidateCacheAsync().GetAwaiter().GetResult();
            return result;
        }

        /// <summary>
        /// Deletes a range of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
            InvalidateCacheAsync().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Implementation of a cached repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class CachedRepository<T, TKey> : CachedRepository<T>, ICachedRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        private readonly IRepository<T, TKey> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cache">The cache.</param>
        public CachedRepository(IRepository<T, TKey> repository, IDistributedCache cache)
            : base(repository, cache)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets an entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey("GetById", id);
            var cachedResult = await GetFromCacheAsync<T>(cacheKey, cancellationToken);
            
            if (cachedResult != null)
            {
                return cachedResult;
            }

            var result = await _repository.GetByIdAsync(id, cancellationToken);
            if (result != null)
            {
                await SetInCacheAsync(cacheKey, result, cancellationToken);
            }
            
            return result;
        }

        /// <summary>
        /// Gets an entity by identifier or throws an exception.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public virtual async Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByIdOrThrowAsync(id, cancellationToken);
        }

        /// <summary>
        /// Deletes an entity from the repository by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteByIdAsync(id, cancellationToken);
            await InvalidateCacheForEntityAsync(id, cancellationToken);
        }

        /// <summary>
        /// Invalidates the cache for an entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public virtual async Task InvalidateCacheForEntityAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var cacheKey = CreateCacheKey("GetById", id);
            await _cache.RemoveAsync(cacheKey, cancellationToken);
        }
    }
}