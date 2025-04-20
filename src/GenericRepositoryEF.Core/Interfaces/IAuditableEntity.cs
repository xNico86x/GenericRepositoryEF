namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for auditable entities.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the date when the entity was created.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the entity.
        /// </summary>
        string? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date when the entity was last modified.
        /// </summary>
        DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who last modified the entity.
        /// </summary>
        string? ModifiedBy { get; set; }
    }
}