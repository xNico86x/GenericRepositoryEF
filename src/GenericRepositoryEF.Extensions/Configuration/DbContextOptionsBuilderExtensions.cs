using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GenericRepositoryEF.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for <see cref="DbContextOptionsBuilder"/>.
    /// </summary>
    public static class DbContextOptionsBuilderExtensions
    {
        /// <summary>
        /// Adds audit interceptors to the DbContext.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="currentUserServiceProvider">A function that provides the current user service.</param>
        /// <returns>The options builder for chaining.</returns>
        public static DbContextOptionsBuilder AddAuditInterceptors(
            this DbContextOptionsBuilder optionsBuilder,
            Func<ICurrentUserService> currentUserServiceProvider)
        {
            optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor(currentUserServiceProvider()));
                
            return optionsBuilder;
        }
        
        /// <summary>
        /// Adds soft delete interceptors to the DbContext.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="currentUserServiceProvider">A function that provides the current user service.</param>
        /// <returns>The options builder for chaining.</returns>
        public static DbContextOptionsBuilder AddSoftDeleteInterceptors(
            this DbContextOptionsBuilder optionsBuilder,
            Func<ICurrentUserService> currentUserServiceProvider)
        {
            optionsBuilder.AddInterceptors(new SoftDeleteSaveChangesInterceptor(currentUserServiceProvider()));
                
            return optionsBuilder;
        }
        
        /// <summary>
        /// Configures logging for the DbContext.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="logLevel">The minimum log level.</param>
        /// <returns>The options builder for chaining.</returns>
        public static DbContextOptionsBuilder ConfigureLogging(
            this DbContextOptionsBuilder optionsBuilder,
            LogLevel logLevel = LogLevel.Information)
        {
            optionsBuilder.EnableSensitiveDataLogging(); // For development
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.LogTo(Console.WriteLine, logLevel);
            
            return optionsBuilder;
        }
        
        /// <summary>
        /// Configures query splitting behavior for the DbContext.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="splitQueries">Whether to split queries.</param>
        /// <returns>The options builder for chaining.</returns>
        public static DbContextOptionsBuilder ConfigureQuerySplitting(
            this DbContextOptionsBuilder optionsBuilder,
            bool splitQueries = true)
        {
            // This will be handled by specific provider options builder
            return optionsBuilder;
        }
    }
}