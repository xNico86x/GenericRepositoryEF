namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        public EntityNotFoundException()
            : base("The specified entity was not found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">The type of entity.</param>
        /// <param name="id">The id of the entity.</param>
        public EntityNotFoundException(Type entityType, object id)
            : base($"Entity of type {entityType.Name} with id {id} was not found.")
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Gets the type of entity.
        /// </summary>
        public Type? EntityType { get; }

        /// <summary>
        /// Gets the id of the entity.
        /// </summary>
        public object? Id { get; }
    }
}