namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for entities that support soft delete.
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date when the entity was deleted.
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }
}