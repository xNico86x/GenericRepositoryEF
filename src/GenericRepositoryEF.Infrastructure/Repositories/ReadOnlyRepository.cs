using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using GenericRepositoryEF.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of the <see cref="IReadOnlyRepository{T, TKey}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    public class ReadOnlyRepository<T, TKey, TContext> : IReadOnlyRepository<T, TKey>
        where T : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T, TKey, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public ReadOnlyRepository(TContext dbContext, ILogger<ReadOnlyRepository<T, TKey, TContext>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting entity of type {EntityType} with id {EntityId}", typeof(T).Name, id);
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting all entities of type {EntityType}", typeof(T).Name);
            return await _dbSet.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting entities of type {EntityType} with predicate", typeof(T).Name);
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Checking if entity of type {EntityType} exists with predicate", typeof(T).Name);
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<int> CountAsync(ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Counting entities of type {EntityType}", typeof(T).Name);
            
            if (specification == null)
            {
                return await _dbSet.CountAsync(cancellationToken);
            }
            
            var queryable = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), specification);
            return await queryable.CountAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting paged entities of type {EntityType}, page {PageNumber}, size {PageSize}", typeof(T).Name, pageNumber, pageSize);
            
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            
            var query = _dbSet.AsQueryable();
            
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(query, specification);
            }
            
            var totalItems = await query.CountAsync(cancellationToken);
            var skip = (pageNumber - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PagedResult<T>(items, totalItems, pageNumber, pageSize);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> GetBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting entities of type {EntityType} with specification", typeof(T).Name);
            
            var queryable = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), specification);
            return await queryable.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<T?> GetSingleBySpecificationAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Getting single entity of type {EntityType} with specification", typeof(T).Name);
            
            var queryable = SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), specification);
            return await queryable.FirstOrDefaultAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Implementation of the <see cref="IReadOnlyRepository{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    public class ReadOnlyRepository<T, TContext> : ReadOnlyRepository<T, int, TContext>, IReadOnlyRepository<T>
        where T : class, IEntity<int>
        where TContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T, TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public ReadOnlyRepository(TContext dbContext, ILogger<ReadOnlyRepository<T, int, TContext>> logger)
            : base(dbContext, logger)
        {
        }
    }
}
