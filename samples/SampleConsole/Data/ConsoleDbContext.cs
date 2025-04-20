using GenericRepositoryEF.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using SampleConsole.Models;

namespace SampleConsole.Data
{
    /// <summary>
    /// Console application database context.
    /// </summary>
    public class ConsoleDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ConsoleDbContext(DbContextOptions<ConsoleDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the customers.
        /// </summary>
        public DbSet<Customer> Customers { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=console.db");
            }

            optionsBuilder.AddInterceptors(
                new AuditSaveChangesInterceptor(() => "Console"),
                new SoftDeleteSaveChangesInterceptor());
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(200);
            });
        }
    }
}