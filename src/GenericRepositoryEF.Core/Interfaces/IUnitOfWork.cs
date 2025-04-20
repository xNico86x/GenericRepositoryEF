namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a unit of work.
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets a repository.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> Repository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a repository with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> Repository<T, TKey>() where T : class, IEntityWithKey<TKey>;

        /// <summary>
        /// Gets a read-only repository.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a read-only repository with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() where T : class, IEntityWithKey<TKey>;

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a transaction.</returns>
        Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}