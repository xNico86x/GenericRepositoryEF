using GenericRepositoryEF.Core.Interfaces;

namespace SampleApi.Models
{
    /// <summary>
    /// Represents a product supplier.
    /// </summary>
    public class Supplier : IEntity, IAuditableEntity, ISoftDelete
    {
        /// <summary>
        /// Gets or sets the supplier identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the supplier name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the supplier contact name.
        /// </summary>
        public string? ContactName { get; set; }

        /// <summary>
        /// Gets or sets the supplier contact email.
        /// </summary>
        public string? ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the supplier phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the supplier address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the products from this supplier.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the deletion time.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who deleted this entity.
        /// </summary>
        public string? DeletedBy { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who created the entity.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last modification time.
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who last modified the entity.
        /// </summary>
        public string? LastModifiedBy { get; set; }
    }
}