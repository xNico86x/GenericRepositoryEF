using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of a read-only repository.
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
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReadOnlyRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of entities.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "ListAllAsync", ex);
            }
        }

        /// <summary>
        /// Gets entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of entities.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ApplySpecification(specification).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "ListAsync", ex);
            }
        }

        /// <summary>
        /// Counts entities using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of entities.</returns>
        public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ApplySpecification(specification).CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "CountAsync", ex);
            }
        }

        /// <summary>
        /// Determines whether any entity exists using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity exists, false otherwise.</returns>
        public async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ApplySpecification(specification).AnyAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "AnyAsync", ex);
            }
        }

        /// <summary>
        /// Gets the first entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity, or null if no entity exists.</returns>
        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "FirstOrDefaultAsync", ex);
            }
        }

        /// <summary>
        /// Gets a single entity using a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A single entity, or null if no entity exists.</returns>
        public async Task<T?> SingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            try
            {
                return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "SingleOrDefaultAsync", ex);
            }
        }

        /// <summary>
        /// Applies a specification to the query.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The query.</returns>
        protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            var query = SpecificationEvaluator.GetQuery(DbSet.AsQueryable(), specification);

            if (!specification.IsTrackingEnabled)
            {
                query = query.AsNoTracking();
            }

            return query;
        }
    }

    /// <summary>
    /// Implementation of a read-only repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class ReadOnlyRepository<T, TKey> : ReadOnlyRepository<T>, IReadOnlyRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReadOnlyRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Gets an entity by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity, or null if the entity does not exist.</returns>
        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet.FindAsync(new object[] { id! }, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(typeof(T), "GetByIdAsync", ex);
            }
        }
    }
}