namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class EntityNotFoundException : RepositoryException
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException() : base("Entity was not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public EntityNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        public EntityNotFoundException(Type entityType) : base($"Entity of type {entityType.Name} was not found.")
        {
            EntityType = entityType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entityId">The entity identifier.</param>
        public EntityNotFoundException(Type entityType, string entityId) : base($"Entity of type {entityType.Name} with id {entityId} was not found.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}