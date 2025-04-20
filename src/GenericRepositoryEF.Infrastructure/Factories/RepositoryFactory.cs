using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repository instances.
    /// </summary>
    public class RepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve repositories.</param>
        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The entity key type.</typeparam>
        /// <returns>A repository for the entity type.</returns>
        public IRepository<TEntity, TKey> CreateRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity, TKey>>();
        }

        /// <summary>
        /// Creates a repository for the specified entity type with int key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A repository for the entity type.</returns>
        public IRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity>>();
        }

        /// <summary>
        /// Creates a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The entity key type.</typeparam>
        /// <returns>A read-only repository for the entity type.</returns>
        public IReadOnlyRepository<TEntity, TKey> CreateReadOnlyRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<IReadOnlyRepository<TEntity, TKey>>();
        }

        /// <summary>
        /// Creates a read-only repository for the specified entity type with int key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A read-only repository for the entity type.</returns>
        public IReadOnlyRepository<TEntity> CreateReadOnlyRepository<TEntity>()
            where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<IReadOnlyRepository<TEntity>>();
        }

        /// <summary>
        /// Creates a null repository for the specified entity type (useful for testing or as fallback).
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The entity key type.</typeparam>
        /// <returns>A null repository for the entity type.</returns>
        public IRepository<TEntity, TKey> CreateNullRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<NullRepository<TEntity, TKey>>();
        }

        /// <summary>
        /// Creates a null repository for the specified entity type with int key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>A null repository for the entity type.</returns>
        public IRepository<TEntity> CreateNullRepository<TEntity>()
            where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<NullRepository<TEntity>>();
        }

        /// <summary>
        /// Gets the DbContext for direct access when needed.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <returns>The database context.</returns>
        public TContext GetDbContext<TContext>() where TContext : DbContext
        {
            return _serviceProvider.GetRequiredService<TContext>();
        }
    }
}