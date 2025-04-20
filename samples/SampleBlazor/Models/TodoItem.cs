using GenericRepositoryEF.Core.Interfaces;

namespace SampleBlazor.Models
{
    /// <summary>
    /// Represents a todo item entity.
    /// </summary>
    public class TodoItem : IEntity, IAuditableEntity
    {
        /// <summary>
        /// Gets or sets the todo item identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this todo item is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the due date.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the priority (1-5).
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user who created the entity.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

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