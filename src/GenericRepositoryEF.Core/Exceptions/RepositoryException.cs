namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a repository operation fails.
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Gets or sets the entity type.
        /// </summary>
        public Type? EntityType { get; set; }

        /// <summary>
        /// Gets or sets the method name.
        /// </summary>
        public string? MethodName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException() : base("A repository error occurred.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public RepositoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="innerException">The inner exception.</param>
        public RepositoryException(Type entityType, string methodName, Exception? innerException = null) 
            : base($"Repository operation {methodName} failed for entity type {entityType.Name}.", innerException)
        {
            EntityType = entityType;
            MethodName = methodName;
        }
    }
}