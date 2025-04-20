namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a concurrency conflict occurs.
    /// </summary>
    public class ConcurrencyException : RepositoryException
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        public ConcurrencyException() : base("A concurrency conflict has occurred.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ConcurrencyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrencyException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConcurrencyException(Type entityType, string entityId, Exception? innerException = null) 
            : base($"A concurrency conflict has occurred for entity of type {entityType.Name} with id {entityId}.", innerException ?? new Exception("Concurrency conflict"))
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}