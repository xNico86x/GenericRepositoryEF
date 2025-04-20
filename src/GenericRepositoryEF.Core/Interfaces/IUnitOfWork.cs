using System.Data;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for the unit of work pattern.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A repository for the entity type.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A read-only repository for the entity type.</returns>
        IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a cached repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A cached repository for the entity type.</returns>
        ICachedRepository<T> CachedRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <returns>A transaction.</returns>
        IDbTransaction BeginTransaction();

        /// <summary>
        /// Begins a transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>A transaction.</returns>
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Begins a transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A transaction.</returns>
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a transaction with the specified isolation level asynchronously.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A transaction.</returns>
        Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of affected entities.</returns>
        int SaveChanges();

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}