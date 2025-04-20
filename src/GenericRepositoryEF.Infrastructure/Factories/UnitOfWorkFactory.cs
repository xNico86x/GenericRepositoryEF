using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating unit of work instances.
    /// </summary>
    public class UnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a unit of work for the specified context type.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <returns>A unit of work.</returns>
        public IUnitOfWork CreateUnitOfWork<TContext>() where TContext : DbContext
        {
            var context = _serviceProvider.GetRequiredService<TContext>();
            return new UnitOfWork<TContext>(context, _serviceProvider);
        }

        /// <summary>
        /// Creates a unit of work for the default context.
        /// </summary>
        /// <returns>A unit of work.</returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
}