using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Sample.Console.Entities
{
    /// <summary>
    /// Base entity class implementing auditable and soft delete interfaces.
    /// </summary>
    public abstract class BaseEntity : IEntity, IAuditableEntity, ISoftDeleteWithUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier as object.
        /// </summary>
        object IEntity.Id
        {
            get => Id;
            set => Id = (int)value;
        }

        /// <summary>
        /// Gets or sets the date the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user that created the entity.
        /// </summary>
        public string CreatedBy { get; set; } = "System";

        /// <summary>
        /// Gets or sets the date the entity was last modified.
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets the user that last modified the entity.
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the user that deleted the entity.
        /// </summary>
        public string? DeletedBy { get; set; }
    }
}