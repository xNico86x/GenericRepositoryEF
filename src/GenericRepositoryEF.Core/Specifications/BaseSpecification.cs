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
            Includes = new List<Expression<Func<T, object>>>();
            IncludeStrings = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
            : this()
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Gets the criteria.
        /// </summary>
        public Expression<Func<T, bool>>? Criteria { get; private set; }

        /// <summary>
        /// Gets the include expressions.
        /// </summary>
        public IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the include strings.
        /// </summary>
        public IReadOnlyList<string> IncludeStrings { get; }

        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        public Expression<Func<T, object>>? OrderBy { get; private set; }

        /// <summary>
        /// Gets the order by descending expression.
        /// </summary>
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        /// <summary>
        /// Gets the group by expression.
        /// </summary>
        public Expression<Func<T, object>>? GroupBy { get; private set; }

        /// <summary>
        /// Gets a value indicating whether tracking is enabled.
        /// </summary>
        public bool IsTrackingEnabled { get; private set; } = true;

        /// <summary>
        /// Gets the skip value.
        /// </summary>
        public int? Skip { get; private set; }

        /// <summary>
        /// Gets the take value.
        /// </summary>
        public int? Take { get; private set; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        public bool IsPagingEnabled => Skip.HasValue && Take.HasValue;

        /// <summary>
        /// Adds a criteria to the specification.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        protected virtual void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Adds an include expression to the specification.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            ((List<Expression<Func<T, object>>>)Includes).Add(includeExpression);
        }

        /// <summary>
        /// Adds an include string to the specification.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        protected virtual void AddInclude(string includeString)
        {
            ((List<string>)IncludeStrings).Add(includeString);
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="skip">The number of entities to skip.</param>
        /// <param name="take">The number of entities to take.</param>
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        /// <summary>
        /// Applies an order by to the specification.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Applies an order by descending to the specification.
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression.</param>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Applies a group by to the specification.
        /// </summary>
        /// <param name="groupByExpression">The group by expression.</param>
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        /// <summary>
        /// Disables tracking.
        /// </summary>
        protected virtual void DisableTracking()
        {
            IsTrackingEnabled = false;
        }
    }
}