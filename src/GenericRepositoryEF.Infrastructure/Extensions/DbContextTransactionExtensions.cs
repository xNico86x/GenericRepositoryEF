using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Reflection;

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
            // Usar reflexión para acceder al campo interno que contiene el DbTransaction
            var transactionType = transaction.GetType();
            
            // Buscar la propiedad o el campo que contiene la transacción DB subyacente
            // No usamos el nombre específico de la clase porque podría cambiar
            var dbTransactionProperty = transactionType.GetProperty("DbTransaction", 
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            if (dbTransactionProperty != null)
            {
                var dbTransaction = dbTransactionProperty.GetValue(transaction) as DbTransaction;
                return dbTransaction ?? throw new InvalidOperationException("DbTransaction property is null.");
            }
            
            // Intentar buscar el primer campo/propiedad que implementa DbTransaction
            foreach (var prop in transactionType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (typeof(DbTransaction).IsAssignableFrom(prop.PropertyType))
                {
                    var dbTransaction = prop.GetValue(transaction) as DbTransaction;
                    return dbTransaction ?? throw new InvalidOperationException("DbTransaction property is null.");
                }
            }
            
            // Si no encontramos la propiedad, buscamos en los campos
            foreach (var field in transactionType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (typeof(DbTransaction).IsAssignableFrom(field.FieldType))
                {
                    var dbTransaction = field.GetValue(transaction) as DbTransaction;
                    return dbTransaction ?? throw new InvalidOperationException("DbTransaction field is null.");
                }
            }
            
            throw new InvalidOperationException("Could not access the underlying DbTransaction.");
        }
    }
}