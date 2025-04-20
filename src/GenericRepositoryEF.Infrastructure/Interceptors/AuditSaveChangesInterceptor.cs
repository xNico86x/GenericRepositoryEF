using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for auditing save changes.
    /// </summary>
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IDateTime _dateTime;
        private readonly ICurrentUserService? _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditSaveChangesInterceptor"/> class.
        /// </summary>
        /// <param name="dateTime">The date time service.</param>
        /// <param name="currentUserService">The current user service.</param>
        public AuditSaveChangesInterceptor(IDateTime dateTime, ICurrentUserService? currentUserService = null)
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

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.ModifiedAt = now;
                    entry.Entity.ModifiedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedAt = now;
                    entry.Entity.ModifiedBy = userId;

                    // Ensure original created values are not modified
                    entry.Property(p => p.CreatedAt).IsModified = false;
                    entry.Property(p => p.CreatedBy).IsModified = false;
                }
            }
        }
    }
}