using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Extensions;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace GenericRepositoryEF.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementation of the unit of work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly ISpecificationEvaluator _specificationEvaluator;
        private readonly IServiceProvider? _serviceProvider;
        private IDbContextTransaction? _transaction;
        private Dictionary<Type, object> _repositories = new();
        private Dictionary<Type, object> _readOnlyRepositories = new();
        private Dictionary<Type, object> _cachedRepositories = new();
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public UnitOfWork(
            DbContext context,
            ISpecificationEvaluator specificationEvaluator,
            IServiceProvider? serviceProvider = null)
        {
            _context = context;
            _specificationEvaluator = specificationEvaluator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A repository for the entity type.</returns>
        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context, _specificationEvaluator);
            }

            return (IRepository<T>)_repositories[type];
        }

        /// <summary>
        /// Gets a read-only repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A read-only repository for the entity type.</returns>
        public IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity
        {
            var type = typeof(T);

            if (!_readOnlyRepositories.ContainsKey(type))
            {
                _readOnlyRepositories[type] = new ReadOnlyRepository<T>(_context, _specificationEvaluator);
            }

            return (IReadOnlyRepository<T>)_readOnlyRepositories[type];
        }

        /// <summary>
        /// Gets a cached repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>A cached repository for the entity type.</returns>
        public ICachedRepository<T> CachedRepository<T>() where T : class, IEntity
        {
            var type = typeof(T);

            if (!_cachedRepositories.ContainsKey(type))
            {
                if (_serviceProvider == null)
                {
                    throw new InvalidOperationException("Service provider is required for cached repositories.");
                }

                var memoryCache = _serviceProvider.GetService<IMemoryCache>();
                var distributedCache = _serviceProvider.GetService<IDistributedCache>();
                var repository = new Repository<T>(_context, _specificationEvaluator);

                _cachedRepositories[type] = new CachedRepository<T>(repository, memoryCache, distributedCache);
            }

            return (ICachedRepository<T>)_cachedRepositories[type];
        }

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <returns>A transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            // Get the underlying transaction from the DbContext transaction
            var dbTransaction = _transaction.GetDbTransaction();
            return dbTransaction;
        }

        /// <summary>
        /// Begins a transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>A transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = _context.Database.BeginTransaction();
            var dbTransaction = _transaction.GetDbTransaction();
            return dbTransaction;
        }

        /// <summary>
        /// Begins a transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A transaction.</returns>
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var dbTransaction = _transaction.GetDbTransaction();
            return dbTransaction;
        }

        /// <summary>
        /// Begins a transaction with the specified isolation level asynchronously.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A transaction.</returns>
        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            var dbTransaction = _transaction.GetDbTransaction();
            return dbTransaction;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        /// <summary>
        /// Commits the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of affected entities.</returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves all changes made in this context to the database asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected entities.</returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Disposes the unit of work.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the unit of work.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}