using System.Linq.Expressions;
using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Specification builder for fluent API.
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
        /// Applies pagination.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> Paginate(int skip, int take)
        {
            ApplyPaging(skip, take);
            return this;
        }

        /// <summary>
        /// Applies pagination with a page number and size.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> Page(int pageNumber, int pageSize)
        {
            ApplyPagingWithPageNumber(pageNumber, pageSize);
            return this;
        }

        /// <summary>
        /// Applies an "order by" expression.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> OrderBy(Expression<Func<T, object>> orderByExpression)
        {
            ApplyOrderBy(orderByExpression);
            return this;
        }

        /// <summary>
        /// Applies an "order by descending" expression.
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> OrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            ApplyOrderByDescending(orderByDescendingExpression);
            return this;
        }

        /// <summary>
        /// Applies a "group by" expression.
        /// </summary>
        /// <param name="groupByExpression">The group by expression.</param>
        /// <returns>The specification builder.</returns>
        public new SpecificationBuilder<T> GroupBy(Expression<Func<T, object>> groupByExpression)
        {
            ApplyGroupBy(groupByExpression);
            return this;
        }

        /// <summary>
        /// Disables tracking of entities.
        /// </summary>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> AsNoTracking()
        {
            DisableTracking();
            return this;
        }

        /// <summary>
        /// Enables tracking of entities.
        /// </summary>
        /// <returns>The specification builder.</returns>
        public SpecificationBuilder<T> WithTracking()
        {
            EnableTracking();
            return this;
        }
    }
}