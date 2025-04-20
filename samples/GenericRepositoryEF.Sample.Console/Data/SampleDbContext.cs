using GenericRepositoryEF.Infrastructure.Interceptors;
using GenericRepositoryEF.Sample.Console.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Sample.Console.Data
{
    /// <summary>
    /// Sample database context.
    /// </summary>
    public class SampleDbContext : DbContext
    {
        private readonly AuditSaveChangesInterceptor? _auditInterceptor;
        private readonly SoftDeleteSaveChangesInterceptor? _softDeleteInterceptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="auditInterceptor">The audit save changes interceptor.</param>
        /// <param name="softDeleteInterceptor">The soft delete save changes interceptor.</param>
        public SampleDbContext(
            DbContextOptions<SampleDbContext> options,
            AuditSaveChangesInterceptor? auditInterceptor = null,
            SoftDeleteSaveChangesInterceptor? softDeleteInterceptor = null)
            : base(options)
        {
            _auditInterceptor = auditInterceptor;
            _softDeleteInterceptor = softDeleteInterceptor;
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Configure the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Category entity
            modelBuilder.Entity<Category>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
                builder.HasQueryFilter(c => !c.IsDeleted);
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
                builder.Property(p => p.Price).HasPrecision(18, 2);
                builder.Property(p => p.Description).HasMaxLength(500);
                builder.HasQueryFilter(p => !p.IsDeleted);

                // Configure relationship between Product and Category
                builder.HasOne(p => p.Category)
                       .WithMany(c => c.Products)
                       .HasForeignKey(p => p.CategoryId)
                       .OnDelete(DeleteBehavior.Restrict);
            });
        }

        /// <summary>
        /// Configure the database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Add interceptors if available
            if (_auditInterceptor != null)
            {
                optionsBuilder.AddInterceptors(_auditInterceptor);
            }

            if (_softDeleteInterceptor != null)
            {
                optionsBuilder.AddInterceptors(_softDeleteInterceptor);
            }
        }
    }
}