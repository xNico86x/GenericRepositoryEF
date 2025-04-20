namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines an entity with a specific key type.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity key.</typeparam>
    public interface IEntityWithKey<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier for this entity.
        /// </summary>
        TKey Id { get; set; }
    }
}