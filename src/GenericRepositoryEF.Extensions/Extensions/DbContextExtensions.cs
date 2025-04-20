using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GenericRepositoryEF.Extensions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Applies global query filters to the model builder.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder.</returns>
        public static ModelBuilder ApplySoftDeleteGlobalFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                    var expression = System.Linq.Expressions.Expression.Equal(property, falseConstant);
                    var lambda = System.Linq.Expressions.Expression.Lambda(expression, parameter);

                    modelBuilder
                        .Entity(entityType.ClrType)
                        .HasQueryFilter(lambda);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Marks entities as modified based on their interfaces.
        /// </summary>
        /// <param name="context">The DbContext.</param>
        /// <param name="dateTimeService">The date time service.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The DbContext.</returns>
        public static DbContext MarkModifiedEntities(this DbContext context, IDateTime dateTimeService, string userId = "System")
        {
            var utcNow = dateTimeService.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.ModifiedAt = utcNow;
                    entry.Entity.ModifiedBy = userId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedAt = utcNow;
                    entry.Entity.ModifiedBy = userId;

                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                }
            }

            foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = utcNow;

                    if (entry.Entity is ISoftDeleteWithUser softDeleteWithUser)
                    {
                        softDeleteWithUser.DeletedBy = userId;
                    }

                    if (entry.Entity is IAuditableEntity auditableEntity)
                    {
                        auditableEntity.ModifiedAt = utcNow;
                        auditableEntity.ModifiedBy = userId;
                    }
                }
            }

            return context;
        }

        /// <summary>
        /// Configures the entity as auditable.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <returns>The entity type builder.</returns>
        public static EntityTypeBuilder<T> ConfigureAsAuditable<T>(this EntityTypeBuilder<T> builder) where T : class, IAuditableEntity
        {
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(255);
            builder.Property(e => e.ModifiedAt).IsRequired();
            builder.Property(e => e.ModifiedBy).IsRequired().HasMaxLength(255);
            return builder;
        }

        /// <summary>
        /// Configures the entity as soft deletable.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <returns>The entity type builder.</returns>
        public static EntityTypeBuilder<T> ConfigureAsSoftDeletable<T>(this EntityTypeBuilder<T> builder) where T : class, ISoftDelete
        {
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.DeletedAt).IsRequired(false);
            return builder;
        }

        /// <summary>
        /// Configures the entity as soft deletable with user.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <returns>The entity type builder.</returns>
        public static EntityTypeBuilder<T> ConfigureAsSoftDeletableWithUser<T>(this EntityTypeBuilder<T> builder) where T : class, ISoftDeleteWithUser
        {
            builder.ConfigureAsSoftDeletable();
            builder.Property(e => e.DeletedBy).IsRequired(false).HasMaxLength(255);
            return builder;
        }
    }
}