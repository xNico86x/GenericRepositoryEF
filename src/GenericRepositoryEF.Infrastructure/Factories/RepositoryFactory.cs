using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repositories.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="dbContext">The database context.</param>
        public RepositoryFactory(IServiceProvider serviceProvider, DbContext dbContext)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc/>
        public IRepository<T> CreateRepository<T>() where T : class, IEntity
        {
            var specificationEvaluator = _serviceProvider.GetRequiredService<ISpecificationEvaluator>();
            return new Repository<T>(_dbContext, specificationEvaluator);
        }

        /// <inheritdoc/>
        public IRepository<T, TKey> CreateRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            var specificationEvaluator = _serviceProvider.GetRequiredService<ISpecificationEvaluator>();
            return new Repository<T, TKey>(_dbContext, specificationEvaluator);
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T> CreateReadOnlyRepository<T>() where T : class, IEntity
        {
            var specificationEvaluator = _serviceProvider.GetRequiredService<ISpecificationEvaluator>();
            return new ReadOnlyRepository<T>(_dbContext, specificationEvaluator);
        }

        /// <inheritdoc/>
        public IReadOnlyRepository<T, TKey> CreateReadOnlyRepository<T, TKey>() where T : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            var specificationEvaluator = _serviceProvider.GetRequiredService<ISpecificationEvaluator>();
            return new ReadOnlyRepository<T, TKey>(_dbContext, specificationEvaluator);
        }
    }
}