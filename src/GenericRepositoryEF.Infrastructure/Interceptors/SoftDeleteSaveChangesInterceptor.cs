using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for soft delete save changes.
    /// </summary>
    public class SoftDeleteSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService? _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeleteSaveChangesInterceptor"/> class.
        /// </summary>
        /// <param name="dateTime">The date time service.</param>
        /// <param name="currentUserService">The current user service.</param>
        public SoftDeleteSaveChangesInterceptor(IDateTime dateTime, ICurrentUserService? currentUserService = null)
        {
            _dateTime = dateTime;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Called before save changes.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result.</param>
        /// <returns>The intercepted result.</returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// Called before save changes asynchronously.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The intercepted result.</returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            var now = _dateTime.UtcNow;
            var userId = _currentUserService?.UserId ?? "System";

            foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    // Change state to modified instead of deleted
                    entry.State = EntityState.Modified;
                    
                    // Set soft delete properties
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    
                    // Set deleted by if entity implements ISoftDeleteWithUser
                    if (entry.Entity is ISoftDeleteWithUser softDeleteWithUser)
                    {
                        softDeleteWithUser.DeletedBy = userId;
                    }
                    
                    // Update modified properties if entity is also auditable
                    if (entry.Entity is IAuditableEntity auditableEntity)
                    {
                        auditableEntity.ModifiedAt = now;
                        auditableEntity.ModifiedBy = userId;
                    }
                }
            }
        }
    }
}