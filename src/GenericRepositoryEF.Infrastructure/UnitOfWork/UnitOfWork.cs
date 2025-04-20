using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementation of the unit of work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly RepositoryFactory _repositoryFactory;
        private readonly ISpecificationEvaluator _specificationEvaluator;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        public UnitOfWork(DbContext context, ISpecificationEvaluator specificationEvaluator)
        {
            _context = context;
            _specificationEvaluator = specificationEvaluator;
            _repositoryFactory = new RepositoryFactory(context, specificationEvaluator);
        }

        /// <summary>
        /// Gets a repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            return _repositoryFactory.CreateRepository<T>();
        }

        /// <summary>
        /// Gets a repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T, TKey> Repository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
            where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.CreateRepository<T, TKey>();
        }

        /// <summary>
        /// Gets a read-only repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        public IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity
        {
            return _repositoryFactory.CreateReadOnlyRepository<T>();
        }

        /// <summary>
        /// Gets a read-only repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        public IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
            where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.CreateReadOnlyRepository<T, TKey>();
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected records.</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The database transaction.</returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}