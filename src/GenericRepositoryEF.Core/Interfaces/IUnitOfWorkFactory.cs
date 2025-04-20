namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a factory for creating units of work.
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Creates a unit of work.
        /// </summary>
        /// <returns>The unit of work.</returns>
        IUnitOfWork CreateUnitOfWork();
    }
}