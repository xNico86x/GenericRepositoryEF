using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Infrastructure.Interceptors;
using GenericRepositoryEF.Infrastructure.Services;
using GenericRepositoryEF.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the generic repository to the service collection.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The configure options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services,
            Action<ServiceConfigurationOptions>? configureOptions = null)
            where TContext : DbContext
        {
            return AddGenericRepository<TContext>(services, configureOptions, null);
        }

        /// <summary>
        /// Adds the generic repository to the service collection.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The configure options.</param>
        /// <param name="configureDbContext">The configure database context.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services,
            Action<ServiceConfigurationOptions>? configureOptions = null,
            Action<DbContextOptionsBuilder>? configureDbContext = null)
            where TContext : DbContext
        {
            var options = new ServiceConfigurationOptions();
            configureOptions?.Invoke(options);

            // Add specification evaluator
            services.AddSingleton<ISpecificationEvaluator, SpecificationEvaluator>();
            services.AddSingleton(typeof(ISpecificationEvaluator<>), typeof(SpecificationEvaluator<>));

            // Add date time service
            if (options.AddDateTimeService)
            {
                services.AddSingleton<IDateTime, DateTimeService>();
            }

            // Add DbContext if not already configured
            if (configureDbContext != null)
            {
                services.AddDbContext<TContext>(configureDbContext);
            }

            // Add unit of work
            services.AddScoped<IUnitOfWork>(provider =>
            {
                var context = provider.GetRequiredService<TContext>();
                var specificationEvaluator = provider.GetRequiredService<ISpecificationEvaluator>();
                return new UnitOfWork(context, specificationEvaluator);
            });

            // Add interceptors
            if (options.AddAuditInterceptor || options.AddSoftDeleteInterceptor)
            {
                services.AddScoped(provider =>
                {
                    var dateTimeService = provider.GetRequiredService<IDateTime>();
                    var currentUserService = provider.GetService<ICurrentUserService>();

                    return (Action<DbContextOptionsBuilder>)(optionsBuilder =>
                    {
                        if (options.AddAuditInterceptor)
                        {
                            optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor(dateTimeService, currentUserService));
                        }

                        if (options.AddSoftDeleteInterceptor)
                        {
                            optionsBuilder.AddInterceptors(new SoftDeleteSaveChangesInterceptor(dateTimeService, currentUserService));
                        }
                    });
                });
            }

            return services;
        }
    }
}