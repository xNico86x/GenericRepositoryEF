using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the <see cref="IDateTime"/> interface.
    /// </summary>
    public class DateTimeService : IDateTime
    {
        /// <summary>
        /// Gets the current date time.
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Gets the current UTC date time.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Gets the current date.
        /// </summary>
        public DateTime Today => DateTime.Today;
    }
}