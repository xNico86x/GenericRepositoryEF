using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GenericRepositoryEF.Infrastructure.Interceptors
{
    /// <summary>
    /// Interceptor to handle soft delete operations.
    /// </summary>
    public class SoftDeleteSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeleteSaveChangesInterceptor"/> class.
        /// </summary>
        /// <param name="currentUserService">The current user service.</param>
        public SoftDeleteSaveChangesInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Called before SaveChanges is invoked.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result of the SaveChanges operation.</param>
        /// <returns>The result of the SaveChanges operation.</returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context != null)
            {
                UpdateSoftDeleteEntities(eventData.Context);
            }

            return base.SavingChanges(eventData, result);
        }

        /// <summary>
        /// Called before SaveChangesAsync is invoked.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <param name="result">The result of the SaveChangesAsync operation.</param>
        /// <returns>The result of the SaveChangesAsync operation.</returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
            {
                UpdateSoftDeleteEntities(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateSoftDeleteEntities(DbContext context)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;

                    // If the entity also implements IAuditableEntity, update the ModifiedAt and ModifiedBy properties
                    if (entry.Entity is IAuditableEntity auditableEntity)
                    {
                        auditableEntity.ModifiedAt = now;
                        auditableEntity.ModifiedBy = _currentUserService.UserId;
                    }
                }
            }
        }
    }
}