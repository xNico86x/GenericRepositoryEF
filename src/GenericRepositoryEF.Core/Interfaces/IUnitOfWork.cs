namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a unit of work for managing transactions and repositories.
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// <summary>
        /// Saves all changes made through the repositories.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
        /// <returns>A repository for the specified entity type.</returns>
        IRepository<TEntity, TKey> Repository<TEntity, TKey>() 
            where TEntity : class, IEntity<TKey> 
            where TKey : IEquatable<TKey>;
            
        /// <summary>
        /// Gets a repository for the specified entity type with an integer identifier.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>A repository for the specified entity type.</returns>
        IRepository<TEntity> Repository<TEntity>() 
            where TEntity : class, IEntity<int>;
            
        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task BeginTransactionAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CommitTransactionAsync(CancellationToken ct = default);
        
        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
