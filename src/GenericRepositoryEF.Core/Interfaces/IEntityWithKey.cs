namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for an entity with a key of a specific type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IEntityWithKey<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        TKey Id { get; set; }
    }
}