using System.Linq.Expressions;

namespace GenericRepositoryEF.Infrastructure.Extensions
{
    /// <summary>
    /// Builder for predicates.
    /// </summary>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate that always returns true.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The predicate.</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// Creates a predicate that always returns false.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The predicate.</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// Combines two predicates with an "or" operation.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="first">The first predicate.</param>
        /// <param name="second">The second predicate.</param>
        /// <returns>The combined predicate.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var invokedExpression = Expression.Invoke(second, first.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(first.Body, invokedExpression), first.Parameters);
        }

        /// <summary>
        /// Combines two predicates with an "and" operation.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="first">The first predicate.</param>
        /// <param name="second">The second predicate.</param>
        /// <returns>The combined predicate.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            var invokedExpression = Expression.Invoke(second, first.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(first.Body, invokedExpression), first.Parameters);
        }

        /// <summary>
        /// Negates a predicate.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <param name="expression">The predicate.</param>
        /// <returns>The negated predicate.</returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }
    }
}