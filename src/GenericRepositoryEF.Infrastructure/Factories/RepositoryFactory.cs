using GenericRepositoryEF.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repositories.
    /// </summary>
    public class RepositoryFactory : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            _serviceProvider = _serviceScope.ServiceProvider;
        }

        /// <summary>
        /// Creates a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The created repository.</returns>
        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity>>();
        }

        /// <summary>
        /// Creates a repository for the specified entity type with a custom key type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity key.</typeparam>
        /// <returns>The created repository.</returns>
        public IRepository<TEntity, TKey> CreateRepository<TEntity, TKey>() 
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity, TKey>>();
        }

        /// <summary>
        /// Creates a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <returns>The created read-only repository.</returns>
        public IReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>() where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<IReadOnlyRepository<TEntity>>();
        }

        /// <summary>
        /// Creates a read-only repository for the specified entity type with a custom key type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the entity key.</typeparam>
        /// <returns>The created read-only repository.</returns>
        public IReadOnlyRepository<TEntity, TKey> CreateReadOnlyRepository<TEntity, TKey>() 
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<IReadOnlyRepository<TEntity, TKey>>();
        }

        /// <summary>
        /// Creates a unit of work.
        /// </summary>
        /// <returns>The created unit of work.</returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        /// <summary>
        /// Disposes the resources used by the factory.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by the factory.
        /// </summary>
        /// <param name="disposing">A value indicating whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _serviceScope.Dispose();
                }

                _disposed = true;
            }
        }
    }
}