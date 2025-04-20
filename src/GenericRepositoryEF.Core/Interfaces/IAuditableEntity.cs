namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for an auditable entity.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified at.
        /// </summary>
        DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        string ModifiedBy { get; set; }
    }
}