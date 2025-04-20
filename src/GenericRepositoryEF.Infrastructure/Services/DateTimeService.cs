using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Infrastructure.Services
{
    /// <summary>
    /// Implementation of date time service.
    /// </summary>
    public class DateTimeService : IDateTime
    {
        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;

        /// <summary>
        /// Gets the current local time.
        /// </summary>
        public DateTime Now => DateTime.Now;
    }
}