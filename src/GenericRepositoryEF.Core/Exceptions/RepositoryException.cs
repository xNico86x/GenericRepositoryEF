namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs in a repository.
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException()
            : base("An error occurred in the repository.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="entityType">The type of entity.</param>
        /// <param name="operation">The operation that failed.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RepositoryException(Type entityType, string operation, Exception innerException)
            : base($"An error occurred in the repository while performing {operation} on entity of type {entityType.Name}.", innerException)
        {
            EntityType = entityType;
            Operation = operation;
        }

        /// <summary>
        /// Gets the type of entity.
        /// </summary>
        public Type? EntityType { get; }

        /// <summary>
        /// Gets the operation that failed.
        /// </summary>
        public string? Operation { get; }
    }
}