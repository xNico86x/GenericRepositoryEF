using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Infrastructure.Transactions
{
    /// <summary>
    /// Implementation of the <see cref="ITransaction"/> interface.
    /// </summary>
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _dbContextTransaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="dbContextTransaction">The database context transaction.</param>
        public Transaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new TransactionException("Commit", ex, true);
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContextTransaction.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new TransactionException("Rollback", ex, true);
            }
        }

        /// <summary>
        /// Disposes the transaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the transaction.
        /// </summary>
        /// <param name="disposing">A value indicating whether the method is called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContextTransaction.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Disposes the transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                await _dbContextTransaction.DisposeAsync();
            }
        }
    }
}