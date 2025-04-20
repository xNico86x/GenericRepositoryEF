using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Adds the generic repository interceptors to the DbContext options builder.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="dateTimeService">The date time service.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="addAuditInterceptor">Whether to add the audit interceptor.</param>
        /// <param name="addSoftDeleteInterceptor">Whether to add the soft delete interceptor.</param>
        /// <returns>The options builder.</returns>
        public static DbContextOptionsBuilder UseGenericRepositoryInterceptors(
            this DbContextOptionsBuilder optionsBuilder,
            IDateTime dateTimeService,
            ICurrentUserService? currentUserService = null,
            bool addAuditInterceptor = true,
            bool addSoftDeleteInterceptor = true)
        {
            if (addAuditInterceptor)
            {
                optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor(dateTimeService, currentUserService));
            }

            if (addSoftDeleteInterceptor)
            {
                optionsBuilder.AddInterceptors(new SoftDeleteSaveChangesInterceptor(dateTimeService, currentUserService));
            }

            return optionsBuilder;
        }

        /// <summary>
        /// Configures the DbContext options to use SQL Server.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The options builder.</returns>
        public static DbContextOptionsBuilder UseGenericRepositorySqlServer(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            return optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        }

        /// <summary>
        /// Configures the DbContext options to use PostgreSQL.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The options builder.</returns>
        public static DbContextOptionsBuilder UseGenericRepositoryNpgsql(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            return optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });
        }

        /// <summary>
        /// Configures the DbContext options to use SQLite.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The options builder.</returns>
        public static DbContextOptionsBuilder UseGenericRepositorySqlite(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString)
        {
            return optionsBuilder.UseSqlite(connectionString, sqliteOptions =>
            {
                sqliteOptions.CommandTimeout(60);
            });
        }

        /// <summary>
        /// Configures the DbContext options to use in-memory database.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="databaseName">The database name.</param>
        /// <returns>The options builder.</returns>
        public static DbContextOptionsBuilder UseGenericRepositoryInMemory(
            this DbContextOptionsBuilder optionsBuilder,
            string databaseName)
        {
            return optionsBuilder.UseInMemoryDatabase(databaseName);
        }
    }
}