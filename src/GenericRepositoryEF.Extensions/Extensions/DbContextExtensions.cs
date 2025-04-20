using System.Linq.Expressions;
using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Extensions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Configures the entity framework model builder to enable soft delete filter for entities that implement <see cref="ISoftDelete"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder for chaining.</returns>
        public static ModelBuilder ConfigureSoftDeleteFilter(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "p");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var falseConst = Expression.Constant(false);
                    var expression = Expression.Equal(property, falseConst);
                    var lambda = Expression.Lambda(expression, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Configures all entities that implement <see cref="ISoftDelete"/> to use a concurrency token.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder for chaining.</returns>
        public static ModelBuilder ConfigureConcurrencyToken(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var property = entityType.FindProperty("RowVersion");
                    if (property != null && property.ClrType == typeof(byte[]))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property("RowVersion")
                            .IsRowVersion();
                    }
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Applies soft delete behavior to a delete operation rather than physically removing the record.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>True if the entity was marked as deleted, false otherwise.</returns>
        public static bool SoftDelete<T>(this DbContext dbContext, T entity) where T : class, ISoftDelete
        {
            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            dbContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        /// <summary>
        /// Applies soft delete behavior to a range of entities rather than physically removing the records.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>The number of entities marked as deleted.</returns>
        public static int SoftDeleteRange<T>(this DbContext dbContext, IEnumerable<T> entities) where T : class, ISoftDelete
        {
            if (entities == null)
            {
                return 0;
            }

            var count = 0;
            var now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = now;
                dbContext.Entry(entity).State = EntityState.Modified;
                count++;
            }

            return count;
        }
    }
}