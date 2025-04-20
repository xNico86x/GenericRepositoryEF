using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Interfaces
{
    /// <summary>
    /// Defines a specification for filtering entities.
    /// </summary>
    /// <typeparam name="T">The type of entity to which the specification applies.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the filtering criteria.
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }
        
        /// <summary>
        /// Gets the include expressions for eager loading.
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }
        
        /// <summary>
        /// Gets the include strings for eager loading.
        /// </summary>
        List<string> IncludeStrings { get; }
        
        /// <summary>
        /// Gets the expression to order by in ascending order.
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }
        
        /// <summary>
        /// Gets the expression to order by in descending order.
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }
        
        /// <summary>
        /// Gets the number of entities to take.
        /// </summary>
        int? Take { get; }
        
        /// <summary>
        /// Gets the number of entities to skip.
        /// </summary>
        int? Skip { get; }
        
        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        bool IsPagingEnabled { get; }
        
        /// <summary>
        /// Gets the grouped include expressions for eager loading with explicit ThenInclude paths.
        /// </summary>
        List<(Expression<Func<T, object>> selector, string navigationPropertyPath)> GroupedIncludes { get; }
        
        /// <summary>
        /// Gets a value indicating whether to use tracking or not.
        /// </summary>
        bool AsNoTracking { get; }
    }
}