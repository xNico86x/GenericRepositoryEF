using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected records.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The database transaction.</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> Repository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity 
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets a read-only repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a read-only repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity 
            where TKey : IEquatable<TKey>;
    }
}