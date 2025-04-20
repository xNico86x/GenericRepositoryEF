using GenericRepositoryEF.Core.Interfaces;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Builder for creating specifications using a fluent API.
    /// </summary>
    /// <typeparam name="T">The type of object to which the specification applies.</typeparam>
    public class SpecificationBuilder<T> : BaseSpecification<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationBuilder{T}"/> class.
        /// </summary>
        public SpecificationBuilder() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationBuilder{T}"/> class with a filter criteria.
        /// </summary>
        /// <param name="criteria">The criteria that determines if an object satisfies the specification.</param>
        public SpecificationBuilder(Expression<Func<T, bool>> criteria) : base(criteria) { }

        /// <summary>
        /// Sets the filter criteria for the specification.
        /// </summary>
        /// <param name="criteria">The criteria that determines if an object satisfies the specification.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public SpecificationBuilder<T> Where(Expression<Func<T, bool>> criteria)
        {
            base.Criteria = criteria;
            return this;
        }

        /// <summary>
        /// Adds an include expression for eager loading.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public SpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
        {
            base.AddInclude(includeExpression);
            return this;
        }

        /// <summary>
        /// Adds a string-based include statement for eager loading.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public SpecificationBuilder<T> Include(string includeString)
        {
            base.AddInclude(includeString);
            return this;
        }
        
        /// <summary>
        /// Adds a grouped include expression for eager loading with explicit ThenInclude path.
        /// </summary>
        /// <param name="selector">The selector expression.</param>
        /// <param name="navigationPropertyPath">The navigation property path for ThenInclude.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public SpecificationBuilder<T> ThenInclude(Expression<Func<T, object>> selector, string navigationPropertyPath)
        {
            base.AddGroupedInclude(selector, navigationPropertyPath);
            return this;
        }

        /// <summary>
        /// Applies ordering in ascending order to the specification.
        /// </summary>
        /// <param name="orderByExpression">The expression to order objects by.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public new SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderByExpression)
        {
            base.ApplyOrderBy(orderByExpression);
            return this;
        }

        /// <summary>
        /// Applies ordering in descending order to the specification.
        /// </summary>
        /// <param name="orderByDescendingExpression">The expression to order objects by.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public new SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            base.ApplyOrderByDescending(orderByDescendingExpression);
            return this;
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="skip">The number of objects to skip.</param>
        /// <param name="take">The maximum number of objects to return.</param>
        /// <returns>The specification builder for method chaining.</returns>
        public new SpecificationBuilder<T> ApplyPaging(int skip, int take)
        {
            base.ApplyPaging(skip, take);
            return this;
        }
        
        /// <summary>
        /// Configures the specification to not track changes to entities.
        /// </summary>
        /// <returns>The specification builder for method chaining.</returns>
        public new SpecificationBuilder<T> AsNoTracking()
        {
            base.SetAsNoTracking();
            return this;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SpecificationBuilder{T}"/> class with the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria that determines if an object satisfies the specification.</param>
        /// <returns>A new specification builder.</returns>
        public static SpecificationBuilder<T> Create(Expression<Func<T, bool>> criteria)
        {
            return new SpecificationBuilder<T>(criteria);
        }
    }
}
