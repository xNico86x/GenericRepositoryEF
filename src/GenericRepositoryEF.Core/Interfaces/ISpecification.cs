using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Non-generic specification evaluator interface.
    /// </summary>
    public interface ISpecificationEvaluator
    {
        /// <summary>
        /// Gets a typed specification evaluator.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The specification evaluator.</returns>
        ISpecificationEvaluator<T> For<T>() where T : class, IEntity;
    }

    /// <summary>
    /// Specification evaluator interface.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ISpecificationEvaluator<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets a query with a specification applied.
        /// </summary>
        /// <param name="inputQuery">The input query.</param>
        /// <param name="specification">The specification.</param>
        /// <returns>The query with the specification applied.</returns>
        IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification);
    }

    /// <summary>
    /// Interface for a specification that can be applied to a query.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface ISpecification<T> where T : class, IEntity
    {
        /// <summary>
        /// Gets the criteria.
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Gets the include expressions.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        List<string> IncludeStrings { get; }

        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// Gets the order by descending expression.
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }

        /// <summary>
        /// Gets the group by expression.
        /// </summary>
        Expression<Func<T, object>>? GroupBy { get; }

        /// <summary>
        /// Gets a value indicating whether the entity should be tracked.
        /// </summary>
        bool IsTrackingEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the query is paging enabled.
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Gets the number of items to skip.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets the number of items to take.
        /// </summary>
        int? Take { get; }
    }
}