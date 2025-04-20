namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Factory for creating repositories.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> GetRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a repository for the specified entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> GetRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity;

        /// <summary>
        /// Gets a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The read-only repository.</returns>
        IReadOnlyRepository<T> GetReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a cached repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The cached repository.</returns>
        ICachedRepository<T> GetCachedRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Gets a cached repository for the specified entity type with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The cached repository.</returns>
        ICachedRepository<T, TKey> GetCachedRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity;
    }
}