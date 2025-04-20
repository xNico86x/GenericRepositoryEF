namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a specification evaluator.
    /// </summary>
    public interface ISpecificationEvaluator
    {
        /// <summary>
        /// Creates a query using the specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="query">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The query with the applied specification.</returns>
        IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class, IEntity;
    }
}