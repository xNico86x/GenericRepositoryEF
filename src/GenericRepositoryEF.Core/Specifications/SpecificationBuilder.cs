using System.Linq.Expressions;
using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Builder for specifications.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public class SpecificationBuilder<T> : BaseSpecification<T> where T : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationBuilder{T}"/> class.
        /// </summary>
        public SpecificationBuilder()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecificationBuilder{T}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public SpecificationBuilder(Expression<Func<T, bool>> criteria)
            : base(criteria)
        {
        }

        /// <summary>
        /// Adds an include expression.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> Include(Expression<Func<T, object>> includeExpression)
        {
            AddInclude(includeExpression);
            return this;
        }

        /// <summary>
        /// Adds an include string.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> Include(string includeString)
        {
            AddInclude(includeString);
            return this;
        }

        /// <summary>
        /// Configures the specification to apply paging.
        /// </summary>
        /// <param name="skip">The number of entities to skip.</param>
        /// <param name="take">The number of entities to take.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> WithPaging(int skip, int take)
        {
            ApplyPaging(skip, take);
            return this;
        }

        /// <summary>
        /// Configures the specification for paging by page number and page size.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> WithPage(int pageNumber, int pageSize)
        {
            return WithPaging((pageNumber - 1) * pageSize, pageSize);
        }

        /// <summary>
        /// Configures the specification to apply an order by.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderByExpression)
        {
            ApplyOrderBy(orderByExpression);
            return this;
        }

        /// <summary>
        /// Configures the specification to apply an order by descending.
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            ApplyOrderByDescending(orderByDescendingExpression);
            return this;
        }

        /// <summary>
        /// Configures the specification to apply a group by.
        /// </summary>
        /// <param name="groupByExpression">The group by expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> GroupBy(Expression<Func<T, object>> groupByExpression)
        {
            ApplyGroupBy(groupByExpression);
            return this;
        }

        /// <summary>
        /// Configures the specification for no tracking.
        /// </summary>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> AsNoTracking()
        {
            DisableTracking();
            return this;
        }
    }
}