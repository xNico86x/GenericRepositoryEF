namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Factory for creating units of work.
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