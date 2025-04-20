using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for auditing entity changes.
    /// </summary>
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService? _currentUserService;
        private readonly DateTime _currentDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditInterceptor"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        public AuditInterceptor(ICurrentUserService? currentUserService = null)
        {
            _currentUserService = currentUserService;
            _currentDateTime = DateTime.UtcNow;
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
                ApplyAuditingRules(eventData.Context);
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
                ApplyAuditingRules(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAuditingRules(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IAuditableEntity auditableEntity)
                    {
                        UpdateAuditableEntity(entry, auditableEntity);
                    }
                }

                if (entry.State == EntityState.Deleted)
                {
                    if (entry.Entity is ISoftDelete softDeleteEntity)
                    {
                        HandleSoftDelete(entry, softDeleteEntity);
                    }
                }
            }
        }

        private void UpdateAuditableEntity(EntityEntry entry, IAuditableEntity auditableEntity)
        {
            if (entry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = _currentDateTime;
                auditableEntity.CreatedBy = _currentUserService?.UserId ?? "System";
            }

            auditableEntity.ModifiedAt = _currentDateTime;
            auditableEntity.ModifiedBy = _currentUserService?.UserId ?? "System";
        }

        private void HandleSoftDelete(EntityEntry entry, ISoftDelete softDeleteEntity)
        {
            entry.State = EntityState.Modified;
            softDeleteEntity.IsDeleted = true;
            softDeleteEntity.DeletedAt = _currentDateTime;
            // Set DeletedBy if entity implements ISoftDeleteWithUser
            if (softDeleteEntity is ISoftDeleteWithUser softDeleteWithUser)
            {
                softDeleteWithUser.DeletedBy = _currentUserService?.UserId ?? "System";
            }
        }
    }
}