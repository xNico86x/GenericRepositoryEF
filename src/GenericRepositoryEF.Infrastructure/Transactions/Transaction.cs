using GenericRepositoryEF.Core.Exceptions;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryEF.Infrastructure.Transactions
{
    /// <summary>
    /// Implementation of a transaction.
    /// </summary>
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="transaction">The database context transaction.</param>
        public Transaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
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
                await _transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new TransactionException("Failed to commit transaction.", ex);
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
                await _transaction.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new TransactionException("Failed to rollback transaction.", ex);
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
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                await _transaction.DisposeAsync().ConfigureAwait(false);
                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes the transaction.
        /// </summary>
        /// <param name="disposing">True if disposing, false if finalizing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }

                _disposed = true;
            }
        }
    }
}