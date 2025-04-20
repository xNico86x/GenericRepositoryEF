namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for an entity that supports soft delete.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether this entity is deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the deleted at.
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }

    /// <summary>
    /// Interface for an entity that supports soft delete with user tracking.
    /// </summary>
    public interface ISoftDeleteWithUser : ISoftDelete
    {
        /// <summary>
        /// Gets or sets the deleted by.
        /// </summary>
        string? DeletedBy { get; set; }
    }
}