using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Infrastructure.Factories;
using GenericRepositoryEF.Infrastructure.Interceptors;
using GenericRepositoryEF.Infrastructure.Repositories;
using GenericRepositoryEF.Infrastructure.Specifications;
using GenericRepositoryEF.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding GenericRepositoryEF to the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds GenericRepositoryEF services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="dbContextOptionsAction">The action to configure the DbContext options.</param>
        /// <param name="currentUserServiceFactory">The factory to create the current user service.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services, 
            Action<DbContextOptionsBuilder> dbContextOptionsAction,
            Func<IServiceProvider, ICurrentUserService?> currentUserServiceFactory) 
            where TContext : DbContext
        {
            // Register DbContext
            services.AddDbContext<TContext>(dbContextOptionsAction);
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<TContext>());

            // Register specification evaluator
            services.AddScoped<ISpecificationEvaluator, SpecificationEvaluator>();

            // Register interceptors
            services.AddScoped<AuditSaveChangesInterceptor>(sp => 
                new AuditSaveChangesInterceptor(currentUserServiceFactory(sp)));
            
            services.AddScoped<SoftDeleteSaveChangesInterceptor>();

            // Register factories and unit of work
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddScoped<IUnitOfWork>(sp => {
                var unitOfWorkFactory = sp.GetRequiredService<IUnitOfWorkFactory>();
                return unitOfWorkFactory.CreateUnitOfWork();
            });

            // Register repositories for direct injection
            services.AddScoped(typeof(IReadOnlyRepository<>), (sp) => {
                var repositoryFactory = sp.GetRequiredService<IRepositoryFactory>();
                var entityType = typeof(IReadOnlyRepository<>).GenericTypeArguments[0];
                var method = repositoryFactory.GetType().GetMethod(nameof(IRepositoryFactory.CreateReadOnlyRepository));
                var genericMethod = method.MakeGenericMethod(entityType);
                return genericMethod.Invoke(repositoryFactory, null);
            });

            services.AddScoped(typeof(IRepository<>), (sp) => {
                var repositoryFactory = sp.GetRequiredService<IRepositoryFactory>();
                var entityType = typeof(IRepository<>).GenericTypeArguments[0];
                var method = repositoryFactory.GetType().GetMethod(nameof(IRepositoryFactory.CreateRepository));
                var genericMethod = method.MakeGenericMethod(entityType);
                return genericMethod.Invoke(repositoryFactory, null);
            });

            return services;
        }

        /// <summary>
        /// Adds GenericRepositoryEF services to the service collection without a current user service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="dbContextOptionsAction">The action to configure the DbContext options.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services, 
            Action<DbContextOptionsBuilder> dbContextOptionsAction) 
            where TContext : DbContext
        {
            return services.AddGenericRepository<TContext>(dbContextOptionsAction, _ => null);
        }

        /// <summary>
        /// Adds GenericRepositoryEF services to the service collection with a specific current user service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="dbContextOptionsAction">The action to configure the DbContext options.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddGenericRepository<TContext>(
            this IServiceCollection services, 
            Action<DbContextOptionsBuilder> dbContextOptionsAction,
            ICurrentUserService currentUserService) 
            where TContext : DbContext
        {
            return services.AddGenericRepository<TContext>(dbContextOptionsAction, _ => currentUserService);
        }
    }
}