namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a unit of work for coordinating multiple operations.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a repository for the specified entity and key types.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> Repository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Gets a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The read-only repository.</returns>
        IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a read-only repository for the specified entity and key types.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
        /// <returns>The read-only repository.</returns>
        IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Saves all changes made in this unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}