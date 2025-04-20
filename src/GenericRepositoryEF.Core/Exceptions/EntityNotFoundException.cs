namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur when an entity is not found.
    /// </summary>
    public class EntityNotFoundException : RepositoryException
    {
        /// <summary>
        /// Gets the name of the entity type.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Gets the identifier of the entity.
        /// </summary>
        public object? EntityId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityName">The name of the entity type.</param>
        public EntityNotFoundException(string entityName)
            : base($"Entity of type '{entityName}' was not found.")
        {
            EntityName = entityName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified entity name and identifier.
        /// </summary>
        /// <param name="entityName">The name of the entity type.</param>
        /// <param name="entityId">The identifier of the entity.</param>
        public EntityNotFoundException(string entityName, object entityId)
            : base($"Entity of type '{entityName}' with id '{entityId}' was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="entityName">The name of the entity type.</param>
        public EntityNotFoundException(string message, string entityName)
            : base(message)
        {
            EntityName = entityName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="entityName">The name of the entity type.</param>
        public EntityNotFoundException(string message, Exception innerException, string entityName)
            : base(message, innerException)
        {
            EntityName = entityName;
        }
    }
}
