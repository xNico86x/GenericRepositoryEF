using GenericRepositoryEF.Core.Interfaces;

namespace SampleApi.Models
{
    /// <summary>
    /// Represents a product category.
    /// </summary>
    public class Category : IEntity, IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the products in this category.
        /// </summary>
        public ICollection<Product> Products { get; set; } = new List<Product>();

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