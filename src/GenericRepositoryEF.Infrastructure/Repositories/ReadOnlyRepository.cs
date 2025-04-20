using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
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
        /// The context.
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        /// The evaluator.
        /// </summary>
        protected readonly ISpecificationEvaluator<T> Evaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="evaluator">The evaluator.</param>
        public ReadOnlyRepository(DbContext context, ISpecificationEvaluator<T> evaluator)
        {
            Context = context;
            Evaluator = evaluator;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <returns>The query.</returns>
        protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return Evaluator.GetQuery(Context.Set<T>().AsQueryable(), specification);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>All entities.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities that match the specification.</returns>
        public virtual async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the first entity that matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the first entity that matches the specification or throws an exception.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="throwIfNotFound">If true, throws an exception if no entity matches the specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The first entity that matches the specification.</returns>
        /// <exception cref="EntityNotFoundException">If no entity matches the specification and throwIfNotFound is true.</exception>
        public virtual async Task<T> FirstOrDefaultAsync(ISpecification<T> specification, bool throwIfNotFound, CancellationToken cancellationToken = default)
        {
            var entity = await FirstOrDefaultAsync(specification, cancellationToken);
            if (entity == null && throwIfNotFound)
            {
                throw new EntityNotFoundException(typeof(T));
            }
            return entity!;
        }

        /// <summary>
        /// Counts entities using a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of entities that match the specification.</returns>
        public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).CountAsync(cancellationToken);
        }

        /// <summary>
        /// Checks if any entity matches the specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if any entity matches the specification.</returns>
        public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).AnyAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Implementation of a read-only repository with a key.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class ReadOnlyRepository<T, TKey> : ReadOnlyRepository<T>, IReadOnlyRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepository{T, TKey}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="evaluator">The evaluator.</param>
        public ReadOnlyRepository(DbContext context, ISpecificationEvaluator<T> evaluator)
            : base(context, evaluator)
        {
        }

        /// <summary>
        /// Gets an entity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public virtual async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        /// <summary>
        /// Gets an entity by identifier or throws an exception.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        /// <exception cref="EntityNotFoundException">If the entity is not found.</exception>
        public virtual async Task<T> GetByIdOrThrowAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(T), id.ToString());
            }
            return entity;
        }
    }
}