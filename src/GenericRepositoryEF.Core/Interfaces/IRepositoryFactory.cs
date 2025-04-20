namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a factory for creating repositories.
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T> CreateRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Creates a repository for the specified entity and key types.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
        /// <returns>The repository.</returns>
        IRepository<T, TKey> CreateRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;

        /// <summary>
        /// Creates a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The read-only repository.</returns>
        IReadOnlyRepository<T> CreateReadOnlyRepository<T>() where T : class, IEntity;

        /// <summary>
        /// Creates a read-only repository for the specified entity and key types.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
        /// <returns>The read-only repository.</returns>
        IReadOnlyRepository<T, TKey> CreateReadOnlyRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>;
    }
}