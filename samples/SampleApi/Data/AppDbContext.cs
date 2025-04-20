using GenericRepositoryEF.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using SampleApi.Models;

namespace SampleApi.Data
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Gets or sets the suppliers.
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app.db");
            }

            optionsBuilder.AddInterceptors(
                new AuditSaveChangesInterceptor(GetCurrentUser),
                new SoftDeleteSaveChangesInterceptor());
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                
                // Configure relationship with Category
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Configure relationship with Supplier
                entity.HasOne(e => e.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(e => e.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configure query filter for soft delete
                entity.HasQueryFilter(p => !p.IsDeleted);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(250);
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Suppliers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactName).HasMaxLength(100);
                entity.Property(e => e.ContactEmail).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(200);

                // Configure query filter for soft delete
                entity.HasQueryFilter(s => !s.IsDeleted);
            });
        }

        private string? GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }
    }
}