namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Evaluates specifications to build IQueryable objects.
    /// </summary>
    public interface ISpecificationEvaluator
    {
        /// <summary>
        /// Gets a queryable that represents all entities in the repository, with the specification applied.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>The queryable.</returns>
        IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification) where T : class, IEntity;
    }
}