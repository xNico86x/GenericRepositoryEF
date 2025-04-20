using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Repositories;
using GenericRepositoryEF.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Reflection;

namespace GenericRepositoryEF.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds repository and unit of work services to the service collection.
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositories<TContext>(
            this IServiceCollection services,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            var options = new RepositoryOptions();
            configure?.Invoke(options);
            
            // Register base repository services
            services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepository<,,>).MakeGenericType(typeof(TContext)));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<,>).MakeGenericType(typeof(TContext)));
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,,>).MakeGenericType(typeof(TContext)));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<,>).MakeGenericType(typeof(TContext)));
            
            // Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            
            // Optionally register cached repositories
            if (options.UseCache)
            {
                services.AddMemoryCache();
                services.Decorate(typeof(IRepository<,>), typeof(CachedRepository<,>));
                services.Decorate(typeof(IRepository<>), typeof(CachedRepository<>));
            }
            
            // Store options
            services.AddSingleton(options);
            
            return services;
        }
        
        /// <summary>
        /// Adds repositories for all entity types in the specified assembly.
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="assembly">The assembly containing entity types.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositoriesFromAssembly<TContext>(
            this IServiceCollection services,
            Assembly assembly,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            services.AddGenericRepositories<TContext>(configure);
            
            // Find all entity types in the assembly
            var entityTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces()
                    .Any(i => i.IsGenericType && 
                             (i.GetGenericTypeDefinition() == typeof(IEntity<>) || 
                              i == typeof(IEntity))));
                              
            foreach (var entityType in entityTypes)
            {
                // Find the IEntity<TKey> interface
                var entityInterface = entityType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>));
                
                if (entityInterface != null)
                {
                    var keyType = entityInterface.GetGenericArguments()[0];
                    
                    // Register repository for this entity type with its key type
                    var repositoryType = typeof(Repository<,,>).MakeGenericType(entityType, keyType, typeof(TContext));
                    var repositoryInterfaceType = typeof(IRepository<,>).MakeGenericType(entityType, keyType);
                    
                    services.AddScoped(repositoryInterfaceType, repositoryType);
                    
                    // If key type is int, also register IRepository<T>
                    if (keyType == typeof(int))
                    {
                        var simpleRepositoryType = typeof(Repository<,>).MakeGenericType(entityType, typeof(TContext));
                        var simpleRepositoryInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                        
                        services.AddScoped(simpleRepositoryInterfaceType, simpleRepositoryType);
                    }
                }
            }
            
            return services;
        }
        
        /// <summary>
        /// Adds SQL Server support for generic repositories.
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositoriesWithSqlServer<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(connectionString));
                
            return services.AddGenericRepositories<TContext>(configure);
        }
        
        /// <summary>
        /// Adds PostgreSQL support for generic repositories.
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositoriesWithPostgreSQL<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseNpgsql(connectionString));
                
            return services.AddGenericRepositories<TContext>(configure);
        }
        
        /// <summary>
        /// Adds SQLite support for generic repositories.
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositoriesWithSQLite<TContext>(
            this IServiceCollection services,
            string connectionString,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseSqlite(connectionString));
                
            return services.AddGenericRepositories<TContext>(configure);
        }
        
        /// <summary>
        /// Adds in-memory database support for generic repositories (useful for testing).
        /// </summary>
        /// <typeparam name="TContext">The type of the database context.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="databaseName">The database name.</param>
        /// <param name="configure">An optional action to configure the repository options.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddGenericRepositoriesWithInMemoryDatabase<TContext>(
            this IServiceCollection services,
            string databaseName,
            Action<RepositoryOptions>? configure = null) 
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
                options.UseInMemoryDatabase(databaseName));
                
            return services.AddGenericRepositories<TContext>(configure);
        }
        
        /// <summary>
        /// Decorates a service registration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="serviceType">The service type to decorate.</param>
        /// <param name="decoratorType">The decorator type.</param>
        /// <returns>The service collection for method chaining.</returns>
        private static IServiceCollection Decorate(
            this IServiceCollection services,
            Type serviceType,
            Type decoratorType)
        {
            var serviceDescriptors = services
                .Where(descriptor => descriptor.ServiceType == serviceType)
                .ToList();
                
            foreach (var descriptor in serviceDescriptors)
            {
                var index = services.IndexOf(descriptor);
                services.Remove(descriptor);
                
                var decoratorDescriptor = new ServiceDescriptor(
                    serviceType,
                    provider =>
                    {
                        // Create the decorated service
                        var decoratorTypeParams = new List<Type>();
                        var constructorTypeParams = new List<object>();
                        
                        // First parameter is the service being decorated
                        decoratorTypeParams.Add(serviceType);
                        constructorTypeParams.Add(CreateInstance(provider, descriptor));
                        
                        // Get remaining constructor parameters from DI
                        var constructor = decoratorType
                            .GetConstructors()
                            .FirstOrDefault(c => c.GetParameters().Length > 0);
                            
                        if (constructor == null)
                        {
                            throw new InvalidOperationException($"No suitable constructor found for decorator type {decoratorType.Name}");
                        }
                        
                        foreach (var parameter in constructor.GetParameters().Skip(1))
                        {
                            decoratorTypeParams.Add(parameter.ParameterType);
                            constructorTypeParams.Add(provider.GetRequiredService(parameter.ParameterType));
                        }
                        
                        // Create generic decorator type if needed
                        Type actualDecoratorType = decoratorType;
                        if (decoratorType.IsGenericTypeDefinition)
                        {
                            var genericArgs = serviceType.GetGenericArguments();
                            actualDecoratorType = decoratorType.MakeGenericType(genericArgs);
                        }
                        
                        // Create and return the decorator
                        return Activator.CreateInstance(actualDecoratorType, constructorTypeParams.ToArray()) 
                            ?? throw new InvalidOperationException($"Failed to create instance of {actualDecoratorType.Name}");
                    },
                    descriptor.Lifetime);
                    
                services.Insert(index, decoratorDescriptor);
            }
            
            return services;
        }
        
        /// <summary>
        /// Creates an instance from a service descriptor.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        /// <param name="descriptor">The service descriptor.</param>
        /// <returns>The created instance.</returns>
        private static object CreateInstance(IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }
            
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(provider);
            }
            
            if (descriptor.ImplementationType == null)
            {
                throw new InvalidOperationException("ImplementationType is null");
            }
            
            return provider.GetRequiredService(descriptor.ImplementationType);
        }
    }
}
