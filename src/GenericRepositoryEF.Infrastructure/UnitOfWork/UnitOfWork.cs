using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Extensions;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace GenericRepositoryEF.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementation of the unit of work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _currentTransaction;
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
            var type = typeof(T);
            var key = $"Repository_{type.FullName}";
            
            if (!_repositories.ContainsKey(key))
            {
                _repositories[key] = _repositoryFactory.CreateRepository<T>(_dbContext);
            }

            return (IRepository<T>)_repositories[key];
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T> ReadOnlyRepository<T>() where T : class, IEntity
        {
            var type = typeof(T);
            var key = $"ReadOnlyRepository_{type.FullName}";
            
            if (!_repositories.ContainsKey(key))
            {
                _repositories[key] = new ReadOnlyRepository<T>(_dbContext);
            }

            return (IReadOnlyRepository<T>)_repositories[key];
        }

        /// <inheritdoc/>
        public ICachedRepository<T> CachedRepository<T>() where T : class, IEntity
        {
            var type = typeof(T);
            var key = $"CachedRepository_{type.FullName}";
            
            if (!_repositories.ContainsKey(key))
            {
                // Crear primero el repositorio base
                var repository = Repository<T>();
                
                // Luego crear el CachedRepository que envuelve al repositorio base
                _repositories[key] = new CachedRepository<T>(repository);
            }

            return (ICachedRepository<T>)_repositories[key];
        }

        /// <inheritdoc/>
        public IRepository<T, TKey> Repository<T, TKey>() where T : class, IEntityWithKey<TKey> where TKey : IEquatable<TKey>
        {
            var type = typeof(T);
            var key = $"RepositoryWithKey_{type.FullName}";
            
            if (!_repositories.ContainsKey(key))
            {
                _repositories[key] = _repositoryFactory.CreateRepository<T, TKey>(_dbContext);
            }

            return (IRepository<T, TKey>)_repositories[key];
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by this instance.
        /// </summary>
        /// <param name="disposing">A value indicating whether this method is being called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public int Complete()
        {
            return SaveChanges();
        }

        /// <inheritdoc/>
        public Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        /// <inheritdoc/>
        public IDbTransaction? GetCurrentTransaction()
        {
            return _currentTransaction?.GetDbTransaction();
        }

        /// <inheritdoc/>
        public async Task<IDbTransaction?> GetCurrentTransactionAsync()
        {
            return _currentTransaction?.GetDbTransaction();
        }

        /// <inheritdoc/>
        public IDbContextTransaction? GetDbContextTransaction()
        {
            return _currentTransaction;
        }

        /// <inheritdoc/>
        public bool HasActiveTransaction()
        {
            return _currentTransaction != null;
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_currentTransaction != null)
            {
                return _currentTransaction.GetDbTransaction();
            }

            _currentTransaction = _dbContext.Database.BeginTransaction(isolationLevel);
            return _currentTransaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return _currentTransaction.GetDbTransaction();
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _currentTransaction.GetDbTransaction();
        }

        /// <inheritdoc/>
        public void CommitTransaction()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            try
            {
                _dbContext.SaveChanges();
                _currentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc/>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc/>
        public void RollbackTransaction()
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                _currentTransaction.Rollback();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc/>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /// <inheritdoc/>
        public void ExecuteInTransaction(Action action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var wasTransactionAlreadyActive = HasActiveTransaction();
            var transaction = BeginTransaction(isolationLevel);

            try
            {
                action();

                if (!wasTransactionAlreadyActive)
                {
                    CommitTransaction();
                }
            }
            catch
            {
                if (!wasTransactionAlreadyActive)
                {
                    RollbackTransaction();
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ExecuteInTransactionAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var wasTransactionAlreadyActive = HasActiveTransaction();
            var transaction = await BeginTransactionAsync(isolationLevel, cancellationToken);

            try
            {
                await action();

                if (!wasTransactionAlreadyActive)
                {
                    await CommitTransactionAsync(cancellationToken);
                }
            }
            catch
            {
                if (!wasTransactionAlreadyActive)
                {
                    await RollbackTransactionAsync(cancellationToken);
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public T ExecuteInTransaction<T>(Func<T> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var wasTransactionAlreadyActive = HasActiveTransaction();
            var transaction = BeginTransaction(isolationLevel);

            try
            {
                var result = action();

                if (!wasTransactionAlreadyActive)
                {
                    CommitTransaction();
                }

                return result;
            }
            catch
            {
                if (!wasTransactionAlreadyActive)
                {
                    RollbackTransaction();
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var wasTransactionAlreadyActive = HasActiveTransaction();
            var transaction = await BeginTransactionAsync(isolationLevel, cancellationToken);

            try
            {
                var result = await action();

                if (!wasTransactionAlreadyActive)
                {
                    await CommitTransactionAsync(cancellationToken);
                }

                return result;
            }
            catch
            {
                if (!wasTransactionAlreadyActive)
                {
                    await RollbackTransactionAsync(cancellationToken);
                }

                throw;
            }
        }

        /// <inheritdoc/>
        public void ResetTracking()
        {
            _dbContext.ChangeTracker.Clear();
        }
    }
}