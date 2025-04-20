namespace GenericRepositoryEF.Extensions
{
    /// <summary>
    /// Options for configuring the repository services.
    /// </summary>
    public class RepositoryOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use caching.
        /// </summary>
        /// <remarks>
        /// When enabled, repositories will be wrapped with <see cref="ICachedRepository{T, TKey}"/>.
        /// </remarks>
        public bool UseCache { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the cache duration in minutes.
        /// </summary>
        public int CacheDurationMinutes { get; set; } = 10;
        
        /// <summary>
        /// Gets or sets a value indicating whether to auto-retry failed database operations.
        /// </summary>
        public bool UseRetry { get; set; } = true;
        
        /// <summary>
        /// Gets or sets the maximum number of retry attempts.
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;
        
        /// <summary>
        /// Gets or sets a value indicating whether to use soft delete.
        /// </summary>
        /// <remarks>
        /// When enabled, entities implementing <see cref="ISoftDelete"/> will be marked as deleted instead of being physically removed.
        /// </remarks>
        public bool UseSoftDelete { get; set; } = true;
        
        /// <summary>
        /// Gets or sets a value indicating whether to track changes on read operations.
        /// </summary>
        /// <remarks>
        /// When false, read operations will use AsNoTracking() for better performance.
        /// </remarks>
        public bool TrackChangesOnReads { get; set; } = false;
        
        /// <summary>
        /// Gets or sets a value indicating whether to auto-commit transactions.
        /// </summary>
        /// <remarks>
        /// When true, SaveChangesAsync will be called automatically when a repository method completes.
        /// </remarks>
        public bool AutoCommit { get; set; } = false;
    }
}
