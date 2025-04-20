namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a concurrency conflict occurs.
    /// </summary>
    public class ConcurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException()
            : base("A concurrency conflict occurred.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConcurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="entityType">The type of entity.</param>
        /// <param name="id">The id of the entity.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ConcurrencyException(Type entityType, object id, Exception innerException)
            : base($"A concurrency conflict occurred while updating entity of type {entityType.Name} with id {id}.", innerException)
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