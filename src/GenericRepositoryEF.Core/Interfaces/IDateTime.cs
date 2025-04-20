namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for obtaining the current date and time.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current date time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current UTC date time.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current date.
        /// </summary>
        DateTime Today { get; }
    }
}