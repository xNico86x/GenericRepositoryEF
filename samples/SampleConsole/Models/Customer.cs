using GenericRepositoryEF.Core.Interfaces;

namespace SampleConsole.Models
{
    /// <summary>
    /// Represents a customer entity.
    /// </summary>
    public class Customer : IEntity, IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer email.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who created the entity.
        /// </summary>
        public string CreatedBy { get; set; } = "System";

        /// <summary>
        /// Gets or sets the last modification time.
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who last modified the entity.
        /// </summary>
        public string? LastModifiedBy { get; set; }
    }
}