namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a transaction fails.
    /// </summary>
    public class TransactionException : RepositoryException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        public TransactionException() : base("A transaction error occurred.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public TransactionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public TransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}