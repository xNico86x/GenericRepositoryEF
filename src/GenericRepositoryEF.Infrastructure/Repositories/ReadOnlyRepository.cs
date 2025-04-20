using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Read-only repository implementation for Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        protected readonly DbContext DbContext;

        /// <summary>
        /// Gets the database set.
        /// </summary>
        protected readonly DbSet<T> DbSet;

        /// <summary>
        /// Gets the specification evaluator.
        /// </summary>
        protected readonly ISpecificationEvaluator SpecificationEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        public ReadOnlyRepository(DbContext context, ISpecificationEvaluator specificationEvaluator)
        {
            DbContext = context;
            DbSet = context.Set<T>();
            SpecificationEvaluator = specificationEvaluator;
        }

        /// <summary>
        /// Finds an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <returns>The found entity or null.</returns>
        public virtual T? Find(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Finds an entity by ID.
        /// </summary>
        /// <param name="id">The ID of the entity to find.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        public virtual async Task<T?> FindAsync(object id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync(new[] { id }, cancellationToken);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All entities.</returns>
        public virtual IReadOnlyList<T> GetAll()
        {
            return DbSet.AsNoTracking().ToList();
        }

        /// <summary>
        /// Gets all entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entities.</returns>
        public virtual IReadOnlyList<T> GetAll(ISpecification<T> specification)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return query.ToList();
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All entities.</returns>
        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets all entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entities.</returns>
        public virtual async Task<IReadOnlyList<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a single entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entity or null.</returns>
        public virtual T? GetSingleOrDefault(ISpecification<T> specification)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return query.SingleOrDefault();
        }

        /// <summary>
        /// Gets a single entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        public virtual async Task<T?> GetSingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the first entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The found entity or null.</returns>
        public virtual T? GetFirstOrDefault(ISpecification<T> specification)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the first entity with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found entity or null.</returns>
        public virtual async Task<T?> GetFirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Counts the number of entities.
        /// </summary>
        /// <returns>The number of entities.</returns>
        public virtual int Count()
        {
            return DbSet.Count();
        }

        /// <summary>
        /// Counts the number of entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The number of entities.</returns>
        public virtual int Count(ISpecification<T> specification)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return query.Count();
        }

        /// <summary>
        /// Counts the number of entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities.</returns>
        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Counts the number of entities with a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities.</returns>
        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return await query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if an entity with the specification exists.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>True if an entity exists, otherwise false.</returns>
        public virtual bool Any(ISpecification<T> specification)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return query.Any();
        }

        /// <summary>
        /// Checks if an entity with the specification exists.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if an entity exists, otherwise false.</returns>
        public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            return await query.AnyAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a paged result.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The paged result.</returns>
        public virtual PagedResult<T> GetPaged(ISpecification<T> specification, int pageNumber, int pageSize)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            var totalCount = query.Count();

            var pagedSpecification = new PagedSpecification<T>(specification, pageNumber, pageSize);
            var pagedQuery = specificationEvaluator.GetQuery(DbSet, pagedSpecification);
            var items = pagedQuery.ToList();

            return PagedResult<T>.Create(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Gets a paged result.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The paged result.</returns>
        public virtual async Task<PagedResult<T>> GetPagedAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var specificationEvaluator = SpecificationEvaluator.For<T>();
            var query = specificationEvaluator.GetQuery(DbSet, specification);
            var totalCount = await query.CountAsync(cancellationToken);

            var pagedSpecification = new PagedSpecification<T>(specification, pageNumber, pageSize);
            var pagedQuery = specificationEvaluator.GetQuery(DbSet, pagedSpecification);
            var items = await pagedQuery.ToListAsync(cancellationToken);

            return PagedResult<T>.Create(items, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Specification for paged queries.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        protected class PagedSpecification<TEntity> : ISpecification<TEntity> where TEntity : class, IEntity
        {
            private readonly ISpecification<TEntity> _baseSpecification;
            private readonly int _pageNumber;
            private readonly int _pageSize;

            /// <summary>
            /// Initializes a new instance of the <see cref="PagedSpecification{TEntity}"/> class.
            /// </summary>
            /// <param name="baseSpecification">The base specification.</param>
            /// <param name="pageNumber">The page number.</param>
            /// <param name="pageSize">The page size.</param>
            public PagedSpecification(ISpecification<TEntity> baseSpecification, int pageNumber, int pageSize)
            {
                _baseSpecification = baseSpecification;
                _pageNumber = pageNumber;
                _pageSize = pageSize;
            }

            /// <summary>
            /// Gets the criteria.
            /// </summary>
            public System.Linq.Expressions.Expression<Func<TEntity, bool>>? Criteria => _baseSpecification.Criteria;

            /// <summary>
            /// Gets the includes.
            /// </summary>
            public List<System.Linq.Expressions.Expression<Func<TEntity, object>>> Includes => _baseSpecification.Includes;

            /// <summary>
            /// Gets the include strings.
            /// </summary>
            public List<string> IncludeStrings => _baseSpecification.IncludeStrings;

            /// <summary>
            /// Gets the order by.
            /// </summary>
            public System.Linq.Expressions.Expression<Func<TEntity, object>>? OrderBy => _baseSpecification.OrderBy;

            /// <summary>
            /// Gets the order by descending.
            /// </summary>
            public System.Linq.Expressions.Expression<Func<TEntity, object>>? OrderByDescending => _baseSpecification.OrderByDescending;

            /// <summary>
            /// Gets the group by.
            /// </summary>
            public System.Linq.Expressions.Expression<Func<TEntity, object>>? GroupBy => _baseSpecification.GroupBy;

            /// <summary>
            /// Gets a value indicating whether tracking is enabled.
            /// </summary>
            public bool IsTrackingEnabled => _baseSpecification.IsTrackingEnabled;

            /// <summary>
            /// Gets a value indicating whether paging is enabled.
            /// </summary>
            public bool IsPagingEnabled => true;

            /// <summary>
            /// Gets the skip.
            /// </summary>
            public int? Skip => (_pageNumber - 1) * _pageSize;

            /// <summary>
            /// Gets the take.
            /// </summary>
            public int? Take => _pageSize;
        }
    }
}