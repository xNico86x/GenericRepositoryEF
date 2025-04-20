using GenericRepositoryEF.Core.Specifications;
using SampleApi.Models;
using System.Linq.Expressions;

namespace SampleApi.Specifications
{
    /// <summary>
    /// Specification for filtering products.
    /// </summary>
    public class ProductSpecification : BaseSpecification<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSpecification"/> class.
        /// </summary>
        public ProductSpecification() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductSpecification"/> class with filtering criteria.
        /// </summary>
        /// <param name="criteria">The filtering criteria.</param>
        public ProductSpecification(Expression<Func<Product, bool>> criteria) : base(criteria) { }

        /// <summary>
        /// Adds an include expression for eager loading.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification AddInclude(Expression<Func<Product, object>> includeExpression)
        {
            base.AddInclude(includeExpression);
            return this;
        }

        /// <summary>
        /// Adds a string-based include statement for eager loading.
        /// </summary>
        /// <param name="includeString">The include string.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification AddInclude(string includeString)
        {
            base.AddInclude(includeString);
            return this;
        }

        /// <summary>
        /// Applies ordering in ascending order to the specification.
        /// </summary>
        /// <param name="orderByExpression">The expression to order objects by.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification ApplyOrderBy(Expression<Func<Product, object>> orderByExpression)
        {
            base.ApplyOrderBy(orderByExpression);
            return this;
        }

        /// <summary>
        /// Applies ordering in descending order to the specification.
        /// </summary>
        /// <param name="orderByDescendingExpression">The expression to order objects by.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification ApplyOrderByDescending(Expression<Func<Product, object>> orderByDescendingExpression)
        {
            base.ApplyOrderByDescending(orderByDescendingExpression);
            return this;
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="skip">The number of objects to skip.</param>
        /// <param name="take">The maximum number of objects to return.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification ApplyPaging(int skip, int take)
        {
            base.ApplyPaging(skip, take);
            return this;
        }

        /// <summary>
        /// Configures the specification to not track changes to entities.
        /// </summary>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification AsNoTracking()
        {
            base.SetAsNoTracking();
            return this;
        }

        /// <summary>
        /// Adds a grouped include expression for eager loading with explicit ThenInclude path.
        /// </summary>
        /// <param name="selector">The selector expression.</param>
        /// <param name="navigationPropertyPath">The navigation property path for ThenInclude.</param>
        /// <returns>The current specification instance.</returns>
        public ProductSpecification AddGroupedInclude(Expression<Func<Product, object>> selector, string navigationPropertyPath)
        {
            base.AddGroupedInclude(selector, navigationPropertyPath);
            return this;
        }

        /// <summary>
        /// Composes this specification with another specification using logical AND.
        /// </summary>
        /// <param name="specification">The specification to compose with.</param>
        /// <returns>A new specification that represents the composition.</returns>
        public ProductSpecification Compose(ProductSpecification specification)
        {
            var newSpec = new ProductSpecification();
            
            // Compose criteria
            if (this.Criteria != null && specification.Criteria != null)
            {
                var parameter = Expression.Parameter(typeof(Product), "p");
                var body = Expression.AndAlso(
                    Expression.Invoke(this.Criteria, parameter),
                    Expression.Invoke(specification.Criteria, parameter)
                );
                newSpec.Criteria = Expression.Lambda<Func<Product, bool>>(body, parameter);
            }
            else if (this.Criteria != null)
            {
                newSpec.Criteria = this.Criteria;
            }
            else if (specification.Criteria != null)
            {
                newSpec.Criteria = specification.Criteria;
            }

            // Copy includes
            foreach (var include in this.Includes)
            {
                newSpec.AddInclude(include);
            }
            foreach (var include in specification.Includes)
            {
                newSpec.AddInclude(include);
            }

            // Copy include strings
            foreach (var includeString in this.IncludeStrings)
            {
                newSpec.AddInclude(includeString);
            }
            foreach (var includeString in specification.IncludeStrings)
            {
                newSpec.AddInclude(includeString);
            }

            // Copy order by
            if (specification.OrderBy != null)
            {
                newSpec.ApplyOrderBy(specification.OrderBy);
            }
            else if (this.OrderBy != null)
            {
                newSpec.ApplyOrderBy(this.OrderBy);
            }

            // Copy order by descending
            if (specification.OrderByDescending != null)
            {
                newSpec.ApplyOrderByDescending(specification.OrderByDescending);
            }
            else if (this.OrderByDescending != null)
            {
                newSpec.ApplyOrderByDescending(this.OrderByDescending);
            }

            // Copy paging
            if (specification.IsPagingEnabled)
            {
                newSpec.ApplyPaging(specification.Skip!.Value, specification.Take!.Value);
            }
            else if (this.IsPagingEnabled)
            {
                newSpec.ApplyPaging(this.Skip!.Value, this.Take!.Value);
            }

            // Copy tracking
            if (specification.AsNoTracking || this.AsNoTracking)
            {
                newSpec.AsNoTracking();
            }

            return newSpec;
        }
    }
}