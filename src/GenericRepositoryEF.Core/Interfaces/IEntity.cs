namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for an entity with an ID.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        object Id { get; set; }
    }

    /// <summary>
    /// Interface for a strongly typed entity with an ID.
    /// </summary>
    /// <typeparam name="TId">The type of the ID.</typeparam>
    public interface IEntity<TId> : IEntity where TId : IEquatable<TId>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        new TId Id { get; set; }
    }

    /// <summary>
    /// Interface for an auditable entity.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the date the entity was created.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user that created the entity.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was last modified.
        /// </summary>
        DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the user that last modified the entity.
        /// </summary>
        string? ModifiedBy { get; set; }
    }

    /// <summary>
    /// Interface for a soft delete entity.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was deleted.
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }

    /// <summary>
    /// Interface for a soft delete entity with user information.
    /// </summary>
    public interface ISoftDeleteWithUser : ISoftDelete
    {
        /// <summary>
        /// Gets or sets the user that deleted the entity.
        /// </summary>
        string? DeletedBy { get; set; }
    }

    /// <summary>
    /// Interface for a concurrency checked entity.
    /// </summary>
    public interface IConcurrencyCheck
    {
        /// <summary>
        /// Gets or sets the concurrency stamp.
        /// </summary>
        byte[] ConcurrencyStamp { get; set; }
    }
}