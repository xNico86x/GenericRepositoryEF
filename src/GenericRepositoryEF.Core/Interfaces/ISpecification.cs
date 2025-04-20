using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a specification for selecting a subset of objects.
    /// </summary>
    /// <typeparam name="T">The type of object to which the specification applies.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the predicate that determines if an object satisfies the specification.
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Gets the list of related entities to be eagerly loaded.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the list of string-based include statements.
        /// </summary>
        List<string> IncludeStrings { get; }

        /// <summary>
        /// Gets the expression for ordering results in ascending order.
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// Gets the expression for ordering results in descending order.
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }

        /// <summary>
        /// Gets the value that represents the maximum number of results to return.
        /// </summary>
        int? Take { get; }

        /// <summary>
        /// Gets the value that represents the number of results to skip.
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        bool IsPagingEnabled { get; }
        
        /// <summary>
        /// Gets the list of grouped include expressions.
        /// </summary>
        List<(Expression<Func<T, object>> selector, string navigationPropertyPath)> GroupedIncludes { get; }
        
        /// <summary>
        /// Gets a value indicating whether to track changes to the entity.
        /// </summary>
        bool AsNoTracking { get; }
    }
}
