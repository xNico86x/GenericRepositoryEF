using System.Data;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets a repository for a given entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a repository for a given entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> Repository<T, TKey>() where T : class, IEntityWithKey<TKey>, IEntity;

        /// <summary>
        /// Gets a read-only repository for a given entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a read-only repository for a given entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() where T : class, IEntityWithKey<TKey>, IEntity;

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The transaction.</returns>
        Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}