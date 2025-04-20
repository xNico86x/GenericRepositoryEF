namespace GenericRepositoryEF.Extensions.DependencyInjection
{
    /// <summary>
    /// Options for configuring the services.
    /// </summary>
    public class ServiceConfigurationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to add the audit interceptor.
        /// </summary>
        public bool AddAuditInterceptor { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to add the soft delete interceptor.
        /// </summary>
        public bool AddSoftDeleteInterceptor { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to add the date time service.
        /// </summary>
        public bool AddDateTimeService { get; set; } = true;
    }
}