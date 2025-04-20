using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating unit of work instances.
    /// </summary>
    public class UnitOfWorkFactory : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope _serviceScope;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
            _serviceProvider = _serviceScope.ServiceProvider;
        }

        /// <summary>
        /// Creates a unit of work for the specified context type.
        /// </summary>
        /// <typeparam name="TContext">The type of database context.</typeparam>
        /// <returns>The created unit of work.</returns>
        public IUnitOfWork CreateUnitOfWork<TContext>() where TContext : DbContext
        {
            var context = _serviceProvider.GetRequiredService<TContext>();
            var logger = _serviceProvider.GetRequiredService<ILogger<UnitOfWork<TContext>>>();
            return new UnitOfWork<TContext>(context, _serviceProvider, logger);
        }

        /// <summary>
        /// Creates a unit of work using the default context.
        /// </summary>
        /// <returns>The created unit of work.</returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        /// <summary>
        /// Disposes the resources used by the factory.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources used by the factory.
        /// </summary>
        /// <param name="disposing">A value indicating whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _serviceScope.Dispose();
                }

                _disposed = true;
            }
        }
    }
}