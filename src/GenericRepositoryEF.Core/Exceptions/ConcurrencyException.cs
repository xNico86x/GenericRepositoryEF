namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a concurrency conflict occurs.
    /// </summary>
    [Serializable]
    public class ConcurrencyException : Exception
    {
        /// <summary>
        /// Gets the entity type.
        /// </summary>
        public Type? EntityType { get; }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        public object? EntityId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class with a specified entity type and identifier.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entityId">The entity identifier.</param>
        public ConcurrencyException(Type entityType, object entityId)
            : base($"Concurrency conflict occurred for entity '{entityType.Name}' with id '{entityId}'.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class with a specified entity type, identifier, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ConcurrencyException(Type entityType, object entityId, Exception innerException)
            : base($"Concurrency conflict occurred for entity '{entityType.Name}' with id '{entityId}'.", innerException)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}