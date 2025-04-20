using System.Linq.Expressions;
using GenericRepositoryEF.Core.Interfaces;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Base class for specifications.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
        /// </summary>
        protected BaseSpecification()
        {
            Criteria = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Gets the criteria.
        /// </summary>
        public Expression<Func<T, bool>>? Criteria { get; private set; }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        public List<string> IncludeStrings { get; } = new();

        /// <summary>
        /// Gets the order by.
        /// </summary>
        public Expression<Func<T, object>>? OrderBy { get; private set; }

        /// <summary>
        /// Gets the order by descending.
        /// </summary>
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        /// <summary>
        /// Gets the group by.
        /// </summary>
        public Expression<Func<T, object>>? GroupBy { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to track the entity.
        /// </summary>
        public bool IsTrackingEnabled { get; private set; } = true;

        /// <summary>
        /// Gets a value indicating whether to enable pagination.
        /// </summary>
        public bool IsPagingEnabled { get; private set; }

        /// <summary>
        /// Gets the skip.
        /// </summary>
        public int? Skip { get; private set; }

        /// <summary>
        /// Gets the take.
        /// </summary>
        public int? Take { get; private set; }

        /// <summary>
        /// Applies an include expression.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        /// <returns>The specification.</returns>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Applies an include string.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        /// <returns>The specification.</returns>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Applies pagination.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>The specification.</returns>
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Applies pagination with a page number and size.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The specification.</returns>
        protected virtual void ApplyPagingWithPageNumber(int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;
            ApplyPaging(skip, pageSize);
        }

        /// <summary>
        /// Applies an "order by" expression.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <returns>The specification.</returns>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Applies an "order by descending" expression.
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression.</param>
        /// <returns>The specification.</returns>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Applies a "group by" expression.
        /// </summary>
        /// <param name="groupByExpression">The group by expression.</param>
        /// <returns>The specification.</returns>
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        /// <summary>
        /// Disables tracking of entities.
        /// </summary>
        /// <returns>The specification.</returns>
        protected virtual void DisableTracking()
        {
            IsTrackingEnabled = false;
        }

        /// <summary>
        /// Enables tracking of entities.
        /// </summary>
        /// <returns>The specification.</returns>
        protected virtual void EnableTracking()
        {
            IsTrackingEnabled = true;
        }
    }
}