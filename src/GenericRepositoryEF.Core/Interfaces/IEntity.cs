namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for entities that have an identifier.
    /// </summary>
    /// <typeparam name="TKey">The type of the identifier.</typeparam>
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier for this entity.
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// Defines a contract for entities with an integer identifier.
    /// </summary>
    /// <remarks>
    /// This is a convenience interface for the common case where entities use integer IDs.
    /// </remarks>
    public interface IEntity : IEntity<int>
    {
    }
}
