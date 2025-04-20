using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repositories.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly DbContext _context;
        private readonly ISpecificationEvaluator _specificationEvaluator;
        private readonly IMemoryCache? _memoryCache;
        private readonly IDistributedCache? _distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public RepositoryFactory(
            DbContext context, 
            ISpecificationEvaluator specificationEvaluator, 
            IMemoryCache? memoryCache = null, 
            IDistributedCache? distributedCache = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _specificationEvaluator = specificationEvaluator ?? throw new ArgumentNullException(nameof(specificationEvaluator));
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            return new Repository<T>(_context, _specificationEvaluator);
        }

        /// <summary>
        /// Gets a repository for the specified entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T, TKey> GetRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
        {
            return new Repository<T, TKey>(_context, _specificationEvaluator);
        }

        /// <summary>
        /// Gets a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The read-only repository.</returns>
        public IReadOnlyRepository<T> GetReadOnlyRepository<T>() where T : class, IEntity
        {
            return new ReadOnlyRepository<T>(_context, _specificationEvaluator);
        }

        /// <summary>
        /// Gets a cached repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The cached repository.</returns>
        public ICachedRepository<T> GetCachedRepository<T>() where T : class, IEntity
        {
            var repository = GetRepository<T>();
            return new CachedRepository<T>(repository, _memoryCache, _distributedCache);
        }

        /// <summary>
        /// Gets a cached repository for the specified entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The cached repository.</returns>
        public ICachedRepository<T, TKey> GetCachedRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
        {
            var repository = GetRepository<T, TKey>();
            return new CachedRepository<T, TKey>(repository, _memoryCache, _distributedCache);
        }
    }
}