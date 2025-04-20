namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a transaction operation fails.
    /// </summary>
    public class TransactionException : RepositoryException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        public TransactionException() : base("A transaction operation failed.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TransactionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public TransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}