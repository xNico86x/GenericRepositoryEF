using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for auditing entity changes.
    /// </summary>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService? _currentUserService;
        private readonly DateTime _currentDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditSaveChangesInterceptor"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        public AuditSaveChangesInterceptor(ICurrentUserService? currentUserService = null)
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
            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = _currentDateTime;
                    entry.Entity.CreatedBy = _currentUserService?.UserId ?? "System";
                }
                
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedAt = _currentDateTime;
                    entry.Entity.LastModifiedBy = _currentUserService?.UserId ?? "System";
                    
                    // Don't modify CreatedAt and CreatedBy
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                }
            }
        }
    }
}