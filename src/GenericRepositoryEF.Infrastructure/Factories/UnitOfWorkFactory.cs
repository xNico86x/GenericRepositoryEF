using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEF.Infrastructure.Factories
{
    /// <summary>
    /// Factory for creating units of work.
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkFactory"/> class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory.</param>
        /// <param name="dbContext">The database context.</param>
        public UnitOfWorkFactory(IRepositoryFactory repositoryFactory, DbContext dbContext)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <inheritdoc/>
        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_dbContext, _repositoryFactory);
        }
    }
}