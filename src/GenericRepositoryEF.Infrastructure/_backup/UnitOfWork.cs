using System.Data;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Infrastructure.UnitOfWorks
{
    /// <summary>
    /// Implementation of the unit of work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
    {
        private readonly DbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        public UnitOfWork(DbContext dbContext, IRepositoryFactory repositoryFactory)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        /// <inheritdoc/>
        public IRepository<T> Repository<T>() where T : class, IEntity
        {
            return _repositoryFactory.GetRepository<T>();
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity
        {
            return _repositoryFactory.GetReadOnlyRepository<T>();
        }

        /// <inheritdoc/>
        public ICachedRepository<T> CachedRepository<T>() where T : class, IEntity
        {
            return _repositoryFactory.GetCachedRepository<T>();
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = _dbContext.Database.BeginTransaction();
            return _transaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = _dbContext.Database.BeginTransaction(isolationLevel);
            return _transaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return _transaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _transaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }

            try
            {
                _dbContext.SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <inheritdoc/>
        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress.");
            }

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Disposes the resources used by this unit of work.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by this unit of work asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by this unit of work asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
            }
        }

        /// <summary>
        /// Disposes the resources used by this unit of work.
        /// </summary>
        /// <param name="disposing">A value indicating whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _dbContext?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}