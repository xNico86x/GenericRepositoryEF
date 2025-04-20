namespace GenericRepositoryEF.Sample.Console.Entities
{
    /// <summary>
    /// Product entity.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public virtual Category? Category { get; set; }
    }
}