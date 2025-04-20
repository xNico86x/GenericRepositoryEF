namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for entities that support soft delete functionality.
    /// </summary>
    /// <remarks>
    /// Soft delete allows entities to be marked as deleted without physically removing them from the database.
    /// </remarks>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether this entity has been deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this entity was deleted.
        /// </summary>
        DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who deleted this entity.
        /// </summary>
        string? DeletedBy { get; set; }
    }
}
