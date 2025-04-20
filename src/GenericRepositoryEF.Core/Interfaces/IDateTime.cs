namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for date time service.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current local time.
        /// </summary>
        DateTime Now { get; }
    }
}