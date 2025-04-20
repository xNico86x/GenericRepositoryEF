namespace GenericRepositoryEF.Sample.Console.Entities
{
    /// <summary>
    /// Category entity.
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the products.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}