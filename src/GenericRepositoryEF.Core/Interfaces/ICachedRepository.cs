namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a repository with caching capabilities.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <typeparam name="TKey">The type of the entity identifier.</typeparam>
    public interface ICachedRepository<T, TKey> : IRepository<T, TKey> 
        where T : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Invalidates all cached data for this repository.
        /// </summary>
        void InvalidateCache();
        
        /// <summary>
        /// Invalidates the cached data for the specified entity.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        void InvalidateCacheItem(TKey id);
    }
    
    /// <summary>
    /// Defines a repository with caching capabilities for entities with an integer identifier.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ICachedRepository<T> : ICachedRepository<T, int>, IRepository<T> 
        where T : class, IEntity<int>
    {
    }
}
