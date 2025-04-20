using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Interface for a specification.
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
        IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        IReadOnlyList<string> IncludeStrings { get; }

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
        /// Gets a value indicating whether tracking is enabled.
        /// </summary>
        bool IsTrackingEnabled { get; }

        /// <summary>
        /// Gets the skip value.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets the take value.
        /// </summary>
        int? Take { get; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        bool IsPagingEnabled { get; }
    }
}