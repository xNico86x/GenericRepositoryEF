using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Infrastructure.UnitOfWorks
{
    /// <summary>
    /// Implementation of the unit of work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
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
            return _repositoryFactory.CreateRepository<T>();
        }

        /// <inheritdoc/>
        public IRepository<T, TKey> Repository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.CreateRepository<T, TKey>();
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity
        {
            return _repositoryFactory.CreateReadOnlyRepository<T>();
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T, TKey> ReadOnlyRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            return _repositoryFactory.CreateReadOnlyRepository<T, TKey>();
        }

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
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