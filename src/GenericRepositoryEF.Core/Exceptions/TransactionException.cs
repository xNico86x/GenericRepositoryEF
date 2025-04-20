namespace GenericRepositoryEF.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs in a transaction.
    /// </summary>
    public class TransactionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        public TransactionException()
            : base("An error occurred in the transaction.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TransactionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public TransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionException"/> class.
        /// </summary>
        /// <param name="operation">The operation that failed.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public TransactionException(string operation, Exception innerException, bool isOperation)
            : base($"An error occurred in the transaction while performing {operation}.", innerException)
        {
            Operation = operation;
        }

        /// <summary>
        /// Gets the operation that failed.
        /// </summary>
        public string? Operation { get; }
    }
}