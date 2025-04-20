using GenericRepositoryEF.Core.Interfaces;
using System.Linq.Expressions;

namespace GenericRepositoryEF.Core.Specifications
{
    /// <summary>
    /// Base implementation of the <see cref="ISpecification{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of object to which the specification applies.</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        /// <inheritdoc />
        public Expression<Func<T, bool>>? Criteria { get; protected set; }
        
        /// <inheritdoc />
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        
        /// <inheritdoc />
        public List<string> IncludeStrings { get; } = new();
        
        /// <inheritdoc />
        public Expression<Func<T, object>>? OrderBy { get; private set; }
        
        /// <inheritdoc />
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }
        
        /// <inheritdoc />
        public int? Take { get; private set; }
        
        /// <inheritdoc />
        public int? Skip { get; private set; }
        
        /// <inheritdoc />
        public bool IsPagingEnabled { get; private set; }
        
        /// <inheritdoc />
        public List<(Expression<Func<T, object>> selector, string navigationPropertyPath)> GroupedIncludes { get; } = new();
        
        /// <inheritdoc />
        public bool AsNoTracking { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
        /// </summary>
        protected BaseSpecification() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class with a filter criteria.
        /// </summary>
        /// <param name="criteria">The criteria that determines if an object satisfies the specification.</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Adds an include expression for eager loading.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Adds a string-based include statement for eager loading.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        
        /// <summary>
        /// Adds a grouped include expression for eager loading with explicit ThenInclude path.
        /// </summary>
        /// <param name="selector">The selector expression.</param>
        /// <param name="navigationPropertyPath">The navigation property path for ThenInclude.</param>
        protected virtual void AddGroupedInclude(Expression<Func<T, object>> selector, string navigationPropertyPath)
        {
            GroupedIncludes.Add((selector, navigationPropertyPath));
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="skip">The number of objects to skip.</param>
        /// <param name="take">The maximum number of objects to return.</param>
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Applies ordering in ascending order to the specification.
        /// </summary>
        /// <param name="orderByExpression">The expression to order objects by.</param>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Applies ordering in descending order to the specification.
        /// </summary>
        /// <param name="orderByDescendingExpression">The expression to order objects by.</param>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        
        /// <summary>
        /// Configures the specification to not track changes to entities.
        /// </summary>
        protected virtual void SetAsNoTracking()
        {
            AsNoTracking = true;
        }
    }
}
