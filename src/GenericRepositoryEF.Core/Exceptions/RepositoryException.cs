namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a repository operation fails.
    /// </summary>
    [Serializable]
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Gets the entity type.
        /// </summary>
        public Type? EntityType { get; }

        /// <summary>
        /// Gets the operation name.
        /// </summary>
        public string? Operation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified entity type, operation name, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="operation">The operation name.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public RepositoryException(Type entityType, string operation, Exception innerException)
            : base($"Repository operation '{operation}' failed for entity '{entityType.Name}'. See inner exception for details.", innerException)
        {
            EntityType = entityType;
            Operation = operation;
        }
    }
}