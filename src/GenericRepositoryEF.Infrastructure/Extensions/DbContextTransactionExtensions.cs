using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for DbContext transactions.
    /// </summary>
    public static class DbContextTransactionExtensions
    {
        /// <summary>
        /// Gets the underlying database transaction.
        /// </summary>
        /// <param name="transaction">The DbContext transaction.</param>
        /// <returns>The database transaction.</returns>
        public static IDbTransaction GetDbTransaction(this IDbContextTransaction transaction)
        {
            // Acceder a la propiedad interna
            var dbTransaction = typeof(DbContextTransaction)
                .GetProperty("DbTransaction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(transaction) as DbTransaction;

            return dbTransaction ?? throw new InvalidOperationException("Could not access the underlying DbTransaction.");
        }
    }
}