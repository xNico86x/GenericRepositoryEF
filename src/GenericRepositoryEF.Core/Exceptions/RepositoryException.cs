namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur in the repository operations.
    /// </summary>
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException() : base("An error occurred in the repository operation.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RepositoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
