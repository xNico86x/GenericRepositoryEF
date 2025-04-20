using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for handling soft delete entities.
    /// </summary>
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService? _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeleteInterceptor"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        public SoftDeleteInterceptor(ICurrentUserService? currentUserService = null)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Called when saving changes.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result.</param>
        /// <returns>The intercepted result.</returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                ApplySoftDeleteRules(eventData.Context);
            }

            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// Called when saving changes asynchronously.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The intercepted result.</returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                ApplySoftDeleteRules(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplySoftDeleteRules(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete softDeleteEntity)
                {
                    entry.State = EntityState.Modified;
                    softDeleteEntity.IsDeleted = true;
                    softDeleteEntity.DeletedAt = DateTime.UtcNow;
                    
                    // Set DeletedBy if the entity implements ISoftDeleteWithUser
                    if (softDeleteEntity is ISoftDeleteWithUser softDeleteWithUser)
                    {
                        softDeleteWithUser.DeletedBy = _currentUserService?.UserId ?? "System";
                    }
                }
            }
        }
    }

    /// <summary>
    /// Extensions for applying soft delete interceptor.
    /// </summary>
    public static class SoftDeleteInterceptorExtensions
    {
        /// <summary>
        /// Applies the soft delete query filter to the model builder.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder.</returns>
        public static ModelBuilder ApplySoftDeleteQueryFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var falseConstant = Expression.Constant(false);
                    var condition = Expression.Equal(property, falseConstant);
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            return modelBuilder;
        }
    }
}