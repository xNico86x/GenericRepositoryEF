using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GenericRepositoryEF.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Implementation of the <see cref="IUnitOfWork"/> interface.
    /// </summary>
    /// <typeparam name="TContext">The type of the database context.</typeparam>
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<UnitOfWork<TContext>> _logger;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public UnitOfWork(
            TContext dbContext,
            IServiceProvider serviceProvider,
            ILogger<UnitOfWork<TContext>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public IRepository<TEntity, TKey> Repository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity, TKey>>();
        }

        /// <inheritdoc />
        public IRepository<TEntity> Repository<TEntity>()
            where TEntity : class, IEntity<int>
        {
            return _serviceProvider.GetRequiredService<IRepository<TEntity>>();
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("Saving changes in unit of work");
            
            try
            {
                return await _dbContext.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error saving changes in unit of work");
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving changes in unit of work");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("Beginning transaction in unit of work");
            
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress for this unit of work.");
            }
            
            _transaction = await _dbContext.Database.BeginTransactionAsync(ct);
        }

        /// <inheritdoc />
        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("Committing transaction in unit of work");
            
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is currently in progress for this unit of work.");
            }
            
            try
            {
                await _dbContext.SaveChangesAsync(ct);
                await _transaction.CommitAsync(ct);
            }
            catch
            {
                await RollbackTransactionAsync(ct);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <inheritdoc />
        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            _logger.LogDebug("Rolling back transaction in unit of work");
            
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_transaction != null)
                {
                    _logger.LogDebug("Disposing of transaction in unit of work");
                    
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
                
                _disposed = true;
            }
        }
    }
}
