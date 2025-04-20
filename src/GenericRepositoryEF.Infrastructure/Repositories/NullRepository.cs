using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;

namespace GenericRepositoryEF.Infrastructure.Repositories
{
    /// <summary>
    /// A repository implementation that does nothing. Useful for testing.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class NullRepository<T> : IRepository<T> where T : class, IEntity
    {
        /// <inheritdoc/>
        public Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <inheritdoc/>
        public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<T>>(new List<T>());
        }

        /// <inheritdoc/>
        public Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <inheritdoc/>
        public Task<T?> SingleOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <inheritdoc/>
        public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        /// <inheritdoc/>
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<int> DeleteAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// A repository implementation that does nothing. Useful for testing.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public class NullRepository<T, TKey> : NullRepository<T>, IRepository<T, TKey>
        where T : class, IEntityWithKey<TKey>, IEntity
    {
        /// <inheritdoc/>
        public Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<T?>(null);
        }

        /// <inheritdoc/>
        public Task<bool> DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }
    }
}