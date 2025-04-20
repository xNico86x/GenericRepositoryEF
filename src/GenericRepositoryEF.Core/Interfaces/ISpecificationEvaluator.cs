namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a service for evaluating specifications to build database queries.
    /// </summary>
    public interface ISpecificationEvaluator
    {
        /// <summary>
        /// Gets the query that represents the specification.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>The resulting query.</returns>
        IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class;
    }
}