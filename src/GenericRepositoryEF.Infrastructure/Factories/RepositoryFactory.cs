using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Specifications;
using GenericRepositoryEF.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating repositories.
    /// </summary>
    public class RepositoryFactory
    {
        private readonly DbContext _context;
        private readonly ISpecificationEvaluator _specificationEvaluator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="specificationEvaluator">The specification evaluator.</param>
        public RepositoryFactory(DbContext context, ISpecificationEvaluator specificationEvaluator)
        {
            _context = context;
            _specificationEvaluator = specificationEvaluator;
        }

        /// <summary>
        /// Creates a repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T> CreateRepository<T>() where T : class, IEntity
        {
            return new Repository<T>(_context, (ISpecificationEvaluator<T>)_specificationEvaluator);
        }

        /// <summary>
        /// Creates a repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        public IRepository<T, TKey> CreateRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
            where TKey : IEquatable<TKey>
        {
            return new Repository<T, TKey>(_context, (ISpecificationEvaluator<T>)_specificationEvaluator);
        }

        /// <summary>
        /// Creates a read-only repository for an entity.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The repository.</returns>
        public IReadOnlyRepository<T> CreateReadOnlyRepository<T>() where T : class, IEntity
        {
            return new ReadOnlyRepository<T>(_context, (ISpecificationEvaluator<T>)_specificationEvaluator);
        }

        /// <summary>
        /// Creates a read-only repository for an entity with a key.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <returns>The repository.</returns>
        public IReadOnlyRepository<T, TKey> CreateReadOnlyRepository<T, TKey>() 
            where T : class, IEntityWithKey<TKey>, IEntity
            where TKey : IEquatable<TKey>
        {
            return new ReadOnlyRepository<T, TKey>(_context, (ISpecificationEvaluator<T>)_specificationEvaluator);
        }
    }
}