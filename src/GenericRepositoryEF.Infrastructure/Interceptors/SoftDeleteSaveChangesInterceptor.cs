using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor for soft delete functionality.
    /// </summary>
    public class SoftDeleteSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService? _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeleteSaveChangesInterceptor"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        public SoftDeleteSaveChangesInterceptor(ICurrentUserService? currentUserService = null)
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
                HandleSoftDelete(eventData.Context);
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
                HandleSoftDelete(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void HandleSoftDelete(DbContext context)
        {
            var entries = context.ChangeTracker
                .Entries<ISoftDelete>()
                .Where(e => e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // Change state from deleted to modified
                entry.State = EntityState.Modified;
                
                // Set the soft delete properties
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
                entry.Entity.DeletedBy = _currentUserService?.UserId ?? "System";
            }
        }
    }
}